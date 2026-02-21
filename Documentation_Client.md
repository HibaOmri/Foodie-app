## 1. Introduction
Ce document détaille la partie "Client" (Front-Office) du projet FoodieApp. L'objectif de ce module est d'offrir une expérience utilisateur fluide et agréable aux clients du restaurant : ils peuvent consulter la carte, passer commande en ligne, réserver des tables et gérer leur profil.

## 2. Fonctionnalités Clés
En tant que développeur responsable de l'interface client, voici les grandes fonctionnalités développées :

### A. Consultation du Menu
- **Affichage des Produits :** Les clients peuvent voir la liste de tous les plats avec leurs images, descriptions et prix.
- **Filtres par Catégories :** Possibilité de filtrer les plats (ex: Burgers, Pizzas, Pâtes) pour faciliter la recherche.

### B. Gestion du Panier (Shopping Cart)
- **Ajout/Suppression :** Ajouter des plats au panier ou en retirer.
- **Mise à jour des quantités :** Modifier le nombre de produits souhaités.
- **Calcul du total :** Le prix total se met à jour dynamiquement.
- **Passage en Caisse (Checkout) :** Pour confirmer la commande de manière simple et intuitive.

### C. Réservation de Table (Booking)
- Les clients peuvent directement réserver une table pour une date, une heure et un nombre de personnes spécifiques depuis le site.

### D. Espace Client (Authentification & Sécurité)
- **Inscription & Connexion :** Un système d'authentification pour sécuriser le compte du client.
- **Suivi des Commandes :** Les utilisateurs connectés peuvent accéder à l'historique et au statut de leurs commandes passées.

---

## 3. Architecture Technique (MVC)

Le projet suit le modèle **MVC (Model-View-Controller)**. Voici comment la partie Client est structurée :

### 📂 Contrôleurs (La Logique)
- **`HomeController.cs`** : Gère l'affichage de la page d'accueil, du menu général, du "À propos" et du contact.
- **`CartController.cs`** : Gère toute la logique liée au panier de l'utilisateur (ajouter des articles en utilisant possiblement les Sessions ou la Base de Données).
- **`AccountController.cs`** : Gère l'inscription, la connexion et la déconnexion des utilisateurs via ASP.NET Core Identity ou Cookies.
- **`BookingsController.cs`** : Gère la création des réservations par le client.

### 📂 Vues (L'Interface)
C'est ce que voit l'utilisateur.
- **`Views/Home/Index.cshtml` & `Menu.cshtml`** : La vitrine du restaurant, développée avec un design moderne.
- **`Views/Cart/Index.cshtml`** : Le résumé du panier pour validation.
- **`Views/Shared/_Layout.cshtml`** : Le gabarit principal incluant la barre de navigation et le pied de page (Footer).

### 🗄️ Modèles (Les Données)
- **`Product.cs` & `Category.cs`** : Utilisés pour lire et afficher les données du menu.
- **`Cart.cs` & `CartItem.cs`** : Structure qui stocke les plats sélectionnés par l'utilisateur avant le paiement.
- **`Booking.cs`** : Modèle pour les données de réservation (date, heure, nombre de personnes).
- **`Order.cs` & `Payment.cs`** : Représente la finalisation du panier (Validation et achat).

---

## 4. Scénario de Démonstration (Pour la soutenance)
Voici un déroulé type pour présenter la partie Client de votre travail :

1. **La Vitrine & L'Accueil** : Commencer par montrer la page d'accueil publique (design attrayant, présentation du restaurant).
2. **Navigation dans le Menu** : Montrer comment le client accède aux produits, et comment fonctionne le filtrage par catégories.
3. **Le Panier en Action** : 
   - Ajouter un plat au panier.
   - Aller sur la page panier et modifier la quantité pour montrer l'ajustement du prix total.
4. **Authentification** : Créer un nouveau compte "Client" ou se connecter avec un compte existant pour pouvoir finaliser la commande.
5. **Réservation (Optionnel)** : Montrer le formulaire de réservation de table.

---

## 5. Points Forts Techniques
- **Expérience Utilisateur (UX) :** L'interface a été conçue pour être "Wow" (moderne, fluide et intuitive) de l'arrivée sur le site jusqu'à l'achat final.
- **Gestion des Sessions/Etat :** Le panier est capable de retenir les produits choisis par l'utilisateur tout au long de sa navigation.
- **Responsive Design :** Le site s'adapte à la taille de l'écran (mobile, tablette, PC) ce qui est parfait pour les utilisateurs qui commandent via smartphone.
