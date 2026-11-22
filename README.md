# 🔐 Mon Coffre

Gestionnaire de mots de passe pour **Windows**.
Tes données sont **chiffrées** et restent **uniquement sur ton ordinateur** : pas de compte, pas de connexion, rien n'est envoyé sur Internet.

## 📥 Installation

1. Va dans l'onglet **Releases** (à droite sur cette page) et télécharge **`MonCoffre.exe`**.
2. Double-clique dessus.
3. Si Windows affiche « Windows a protégé votre ordinateur » : clique sur **Informations complémentaires**, puis **Exécuter quand même**. C'est normal pour une petite application indépendante.

## 🚀 Premier lancement

1. Au démarrage, Mon Coffre te demande de **créer un mot de passe maître** (8 caractères minimum).
2. C'est ce mot de passe qui ouvre ton coffre à chaque lancement.

> ⚠️ **Si tu oublies ton mot de passe maître, tes données sont perdues.** Personne ne peut les récupérer, pas même moi.

## 📖 Comment ça marche

**Les cases** (colonne de gauche) : une case par site ou par compte, par exemple « Réseaux sociaux » ou « Banque ».
- **+ Ajouter** crée une case, **Renommer** change son nom, **Supprimer** l'envoie à la corbeille.
- **▲ / ▼** change l'ordre.

**Les lignes** (tableau de droite) : chaque case contient des lignes (Site, Identifiant, Mot de passe…).
- **+ Créer une ligne** ajoute une ligne. Les lignes de type « mot de passe » sont masquées.
- **Générer un mot de passe** en crée un solide, que tu peux copier directement.
- Une barre de force t'indique si ton mot de passe est faible ou fort.

**Verrouiller** : bouton **🔒 Verrouiller**, ou **Ctrl+L**, ou **Échap deux fois**. Le coffre se verrouille aussi tout seul après quelques minutes d'inactivité.

**Corbeille** : ce que tu supprimes reste 30 jours dans la corbeille avant d'être effacé pour de bon.

**Sauvegarde** : **⬇ Sauvegarde** crée un fichier `.coffre` chiffré à garder en lieu sûr. **⬆ Importer** le restaure (il remplace le contenu actuel).

**Sécurité** : tu peux cacher la fenêtre des captures d'écran et la masquer dès que tu cliques ailleurs.

## 🆓 Version gratuite

- Coffre chiffré, cases et lignes à volonté
- Générateur de mots de passe
- Corbeille, verrouillage automatique, sauvegarde et import
- 6 styles de couleurs : Bleu, Vert, Rouge, Gris acier, Turquoise, Graphite
- 3 fonds d'écran : image perso, Bulles, Matrix

## ⭐ Version Pro

- Tous les styles de couleurs et la **couleur libre**
- **11 fonds animés** en plus
- **Icônes** personnalisées pour chaque case
- **Audit des mots de passe** : repère les mots de passe faibles et ceux réutilisés, avec un score sur 100
- **Sauvegardes automatiques** à chaque fermeture (les 20 dernières sont gardées)

### Activer une licence Pro
1. Clique sur **📜 Licence**, puis **Activer une licence…**
2. Entre le **nom** et la **clé** reçus à l'achat (le nom doit être écrit exactement comme à l'achat).
3. Chaque clé ne s'active **qu'une seule fois**. Si tu la désactives, il faut en redemander une.

👉 **Pour obtenir une licence : [écris ton moyen de contact ici]**

## ❓ Questions fréquentes

**Où sont mes données ?**
Dans `%AppData%\MonCoffre` sur ton PC. Pour ouvrir ce dossier : touches **Windows + R**, tape `%AppData%\MonCoffre`, puis Entrée.

**Où sont les sauvegardes automatiques (Pro) ?**
Dans `%AppData%\MonCoffre\Sauvegardes`.

**Est-ce que l'application envoie quelque chose sur Internet ?**
Non, sauf au moment d'activer une licence Pro, où elle vérifie que la clé n'a pas déjà été utilisée.

**Je veux compiler moi-même.**
Mets `MonCoffre.cs` (et `MonCoffre.ico`) dans un dossier avec `Creer-MonCoffre.bat`, puis double-clique sur le `.bat`. Aucune installation n'est nécessaire : le compilateur est déjà dans Windows.

## ⚖️ Droits

© Mon Coffre. Tous droits réservés. Le code est visible, mais il n'est pas sous licence libre : toute réutilisation ou redistribution nécessite mon accord.
