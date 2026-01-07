# 🎓 Documentation Technique - Module Administration
**Projet :** FoodieApp  
**Rôle :** Administrateur (Admin)

---

## 1. Introduction
Ce document détaille la partie "Administration" du projet FoodieApp. L'objectif de ce module est de fournir une interface sécurisée permettant au gestionnaire du restaurant de contrôler le contenu de l'application (produits, catégories) et de suivre l'activité (commandes).

## 2. Fonctionnalités Clés
En tant qu'administrateur, mes responsabilités techniques couvrent les points suivants :

### A. Gestion des Produits (CRUD)
Le cœur de mon travail est le système **CRUD** (Create, Read, Update, Delete) pour les plats du restaurant.
- **Create** : Ajouter un nouveau plat avec une image, un prix et une description.
- **Read** : Consulter la liste complète des plats disponibles.
- **Update** : Modifier le prix ou les détails d'un plat existant.
- **Delete** : Supprimer un plat du menu.

### B. Sécurité & Contrôle d'Accès
Une partie critique est la sécurisation de l'accès. Seuls les utilisateurs authentifiés avec le rôle **"Admin"** peuvent accéder aux pages de gestion.
- **Technologie utilisée** : ASP.NET Core Identity / Cookie Authentication.
- **Mécanisme** : Attribut `[Authorize(Roles = "Admin")]` placé sur les contrôleurs sensibles.

### C. Gestion des Catégories
Organisation des produits par familles (Burgers, Pizzas, Pâtes, etc.) pour structurer le menu client.

### D. Suivi des Commandes
Vue d'ensemble sur toutes les commandes passées par les clients, incluant les détails (utilisateurs, produits commandés, date).

---

## 3. Architecture Technique (MVC)

Le projet suit le modèle **MVC (Model-View-Controller)**. Voici comment ma partie est structurée :

### 📂 Contrôleurs (La Logique)
C'est le cerveau de l'administration.
- **`ProductsController.cs`** : Gère toute la logique des produits. Il vérifie si l'utilisateur est admin, valide les données saisies (prix positif, nom non vide) et parle à la base de données.
- **`CategoriesController.cs`** : Gère la logique des catégories.
- **`OrdersController.cs`** : Récupère les commandes depuis la base de données.

*Exemple de code clé (Sécurité)* :
```csharp
[Authorize(Roles = "Admin")] // <-- Verrou de sécurité
public class ProductsController : Controller
{
    // Seul un admin peut exécuter ces méthodes
}
```

### 📂 Vues (L'Interface)
C'est ce que l'utilisateur voit.
- **`Views/Products/Index.cshtml`** : Le tableau de bord affichant la liste des plats.
- **`Views/Products/Create.cshtml`** : Le formulaire d'ajout avec gestion de l'upload d'image.
- **`Views/Shared/_Layout.cshtml`** : Contient la logique d'affichage conditionnel du menu (le lien "Admin" n'apparaît que si on est connecté en tant qu'admin).

### 🗄️ Modèles (Les Données)
- **`Product.cs`** : Définit la structure d'un plat (Id, Nom, Prix, ImageUrl...).
- **`Category.cs`** : Définit la structure d'une catégorie.

---

## 4. Scénario de Démonstration (Pour la soutenance)
Voici un déroulé type pour présenter votre travail :

1.  **Connexion Sécurisée** : Montrer que l'accès à `/Products` est bloqué pour un visiteur normal. Se connecter avec le compte `admin`.
2.  **Tableau de Bord** : Montrer l'apparition du menu "Admin Produits" après connexion.
3.  **Ajout d'un Produit** : Créer un "Burger Spécial Soutenance", uploader une image, et mettre un prix.
4.  **Vérification** : Aller sur la page d'accueil (partie client) pour montrer que le nouveau burger est immédiatement visible pour les clients.
5.  **Modification/Suppression** : Modifier le prix du burger, puis le supprimer pour montrer la robustesse du système.

---

## 5. Points Forts Techniques
- **Upload d'images** : Gestion des fichiers et stockage sécurisé dans le dossier `wwwroot/images`.
- **Entity Framework Core** : Utilisation de l'ORM pour interagir avec SQL Server sans écrire de SQL brut.
- **Validation** : Protection contre les données invalides (ex: empêcher un prix négatif).
