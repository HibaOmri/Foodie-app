# ☁️ Guide d'Hébergement Azure pour FoodieApp

Ce guide vous accompagne pas à pas pour mettre votre site en ligne sur Microsoft Azure.

---

## Etape 1 : Créer la Base de Données (Azure SQL) 🗄️

Votre base de données actuelle est sur votre PC. Il faut en créer une dans le cloud.

1. Connectez-vous sur [portal.azure.com](https://portal.azure.com).
2. Cliquez sur **"Créer une ressource"** > Recherchez **"SQL Database"**.
3. Cliquez sur **Créer**.
4. Remplissez le formulaire :
   - **Abonnement** : (Votre abonnement étudiant/Azure).
   - **Groupe de ressources** : Créez-en un nouveau (ex: `FoodieResourceGroup`).
   - **Nom de la base** : `FoodieDB` (ou un nom unique).
   - **Serveur** : Cliquez sur "Créer nouveau". Donnez un nom (ex: `foodie-server-hiba`), un login admin (ex: `foodieadmin`) et un mot de passe complexe (notez-le bien !). **Lieu** : "France Central" ou "West Europe".
   - **Niveau tarifaire** : ⚠️ **IMPORTANT** pour ne pas payer cher. Choisissez "Standard" ou, si disponible, cherchez l'option "Basic" ou "Serverless" qui est moins chère. Pour les étudiants, il y a souvent une offre gratuite (Free Tier).
5. Cliquez sur **Vérifier et créer** > **Créer**.
6. ⏳ Attendez que le déploiement soit terminé.

---

## Etape 2 : Configurer le Pare-feu de la Base de Données 🛡️

Pour que votre ordinateur (et Azure) puisse parler à la base :

1. Allez sur la ressource **SQL Database** ou **Serveur SQL** que vous venez de créer.
2. Dans le menu de gauche, cherchez **"Mise en réseau"** (Networking) ou **"Pare-feu"**.
3. Cochez l'option : **"Autoriser les services et les ressources Azure à accéder à ce serveur"** (C’est crucial pour que votre site web marche).
4. (Optionnel) Cliquez sur **"Ajouter l'adresse IP de votre client"** pour pouvoir vous connecter depuis votre PC (utile si vous voulez voir les données depuis chez vous).
5. **Enregistrez**.

---

## Etape 3 : Récupérer la Chaîne de Connexion 🔗

1. Toujours sur la page de votre **Base de données SQL**.
2. Cliquez sur **"Chaînes de connexion"** (Connection strings) dans le menu.
3. Copiez la chaîne sous l'onglet **ADO.NET**. Elle ressemble à :
   `Server=tcp:foodie-server-hiba.database.windows.net,1433;Initial Catalog=FoodieDB;Persist Security Info=False;User ID={votre_login};Password={votre_mot_de_passe};...`
4. Collez-la dans un bloc-notes temporaire. Remplacez `{your_password}` par le vrai mot de passe que vous avez choisi à l'étape 1.

---

## Etape 4 : Créer le Site Web (App Service) 🌐

1. Retournez sur l'accueil du portail Azure.
2. Cliquez sur **"Créer une ressource"** > **"Web App"** (Application Web).
3. Remplissez :
   - **Groupe de ressources** : Sélectionnez le même que pour la base (`FoodieResourceGroup`).
   - **Nom** : Donnez un nom unique (ex: `foodie-app-hiba`). Ce sera l'adresse de votre site (`foodie-app-hiba.azurewebsites.net`).
   - **Publier** : Code.
   - **Pile d'exécution** (Runtime stack) : **.NET 8 (LTS)**.
   - **Système d'exploitation** : Windows.
   - **Région** : La même que la base ("France Central" ou "West Europe").
   - **Plan tarifaire** : Choisissez **F1 (Gratuit)** si disponible (onglet Dev/Test), ou **B1**.
4. Cliquez sur **Vérifier et créer** > **Créer**.

---

## Etape 5 : Connecter le Site à la Base de Données 🔌

C'est ici que la magie opère. On va dire au site d'utiliser la base Azure et non la base locale.

1. Une fois le déploiement terminé, allez sur la ressource **App Service** (votre site web).
2. Dans le menu de gauche, cherchez **"Configuration"** (ou "Variables d'environnement").
3. Cliquez sur **"Nouvelle chaîne de connexion"** (New connection string).
   - **Nom** : `DefaultConnection` (⚠️ Doit être EXACTEMENT le même nom que dans votre `appsettings.json`).
   - **Valeur** : Collez la chaîne de connexion Azure que vous avez préparée à l'étape 3 (avec le vrai mot de passe).
   - **Type** : SQLServer.
4. Cliquez sur **OK** puis sur **Enregistrer** (Save) en haut de la page. Confirmez.

---

## Etape 6 : Publier votre Code (Deployment) 🚀

Méthode la plus simple avec VS Code :

1. Installez l'extension **"Azure App Service"** dans VS Code (si ce n'est pas déjà fait).
2. Cliquez sur le logo Azure (A) dans la barre latérale gauche.
3. Connectez-vous à votre compte Azure.
4. Dépliez la liste **"App Services"**. Vous devriez voir votre site (`foodie-app-hiba`).
5. Faites un clic-droit sur le dossier de votre projet (`FoodieApp`) dans l'explorateur de fichiers VS Code, et choisissez **"Deploy to Web App..."**.
6. Sélectionnez votre site Azure dans la liste.
7. Confirmez ("Deploy").

VS Code va compiler le projet, l'envoyer sur Azure et l'ouvrir.

### Alternative (Si vous n'avez pas VS Code configuré) :

1. Dans votre terminal, tapez : `dotnet publish -c Release -o ./publish`.
2. Allez dans le dossier `publish` créé.
3. Sélectionnez tous les fichiers -> Clic droit -> **Envoyer vers** -> **Dossier compressé (zip)**.
4. Sur le Portail Azure > Votre App Service > Menu de gauche **"Advanced Tools" (Kudu)** > Go.
5. Dans la nouvelle fenêtre : **Tools** > **Zip Push Deploy**.
6. Glissez-déposez votre fichier `.zip` dans la zone prévue.

---

## Félicitations ! 🎉

Allez sur `https://votre-site.azurewebsites.net`.
Au premier chargement, cela peut prendre un peu de temps car la base de données va se créer automatiquement (grâce à `context.Database.EnsureCreated()` dans votre code).

Si vous avez une erreur, vérifiez bien la **Chaîne de connexion** dans l'étape 5 et le **Pare-feu** dans l'étape 2.
