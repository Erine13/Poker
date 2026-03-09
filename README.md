# Poker

## Introduction 
Ce projet a été donnée durant une première année de BTS SIO. Le contexte de ce projet est que la société HsH souhaite offrir un espace de détente proposant de petits jeux sur l'ordinateur mais certains jeux marchent mal. Nous avons du assurer la maintenance corrective d'un programme s’inspirant du jeu du Poker. 

Le poker est un jeu !!! A revoir pour règle poker
où un tirage aléatoire de 5 cartes est effectué. Il faudra s’assurer que chaque carte est unique, et ensuite, l’utilisateur aura la possibilité, soit de conserver son jeu, soit d’échanger aux plus quatre cartes, et obtenir ainsi une nouvelle main

## Fonctions à compléter ##

### 1- Tirage cartes

Pour la fonction du tirage de cartes : private static carte tirage() 

   ```public static carte tirage()
   {
       carte carte_tire;
       Random random = new Random();
       int v = random.Next(0, 13); //Prend une valeur aléatoire entre 0 et 12, correspondant à une valeur de carte présent dans la tableau 0 = A
       int f = random.Next(0, 4); //Prend une valeur aléatoire entre 0 et 3, correspondant à une famille (coeur,pique...)
       carte_tire.valeur = valeurs[v]; //La valeur de la carte prends la valeur tiré (noté v) dans la tableau valeurs 
       carte_tire.famille = familles[f]; //La famille prend la famille tiré (noté f) dans le tableau familles


       return carte_tire;

   }```



Cette fonction sert à genéré aléatoirement une carte que ce soit pour sa valeur ou pour sa famille.

<img width="1098" height="293" alt="image" src="https://github.com/user-attachments/assets/0d0b234f-a15e-43a7-b4ee-d06b2c25a5cc" />

### 2 - private static bool carteUnique(carte uneCarte, carte[] unJeu, int numero)

Cette fonction indique si une carte est déjà présente dans le jeu 

### 3 - private static void tirageDuJeu(carte[] unJeu)

### 4 - private static void echangeDeCartes(carte[] unJeu, int[] e)

### 5 - private static combinaison chercheCombinaison(carte[] unJeu)

### 6 - private static void echangeDeCartes(carte[] unJeu, int[] e)
