# Poker

## Introduction 
Ce projet a été donnée durant une première année de BTS SIO. Le contexte de ce projet est que la société HsH souhaite offrir un espace de détente proposant de petits jeux sur l'ordinateur mais certains jeux marchent mal. Nous avons du assurer la maintenance corrective d'un programme s’inspirant du jeu du Poker. 

Le poker est un jeu !!! A revoir pour règle poker
où un tirage aléatoire de 5 cartes est effectué. Il faudra s’assurer que chaque carte est unique, et ensuite, l’utilisateur aura la possibilité, soit de conserver son jeu, soit d’échanger aux plus quatre cartes, et obtenir ainsi une nouvelle main

## Fonctions à compléter ##

### 1- Tirage cartes

Cette fonction du tirage de cartes (private static carte tirage()) permet de tirer aléatoirement une carte du jeu avec une valeur et une famille.

   ```C#
public static carte tirage()
   {
       carte carte_tire;
       Random random = new Random();
       int v = random.Next(0, 13); //Prend une valeur aléatoire entre 0 et 12, correspondant à une valeur de carte présent dans la tableau 0 = A
       int f = random.Next(0, 4); //Prend une valeur aléatoire entre 0 et 3, correspondant à une famille (coeur,pique...)
       carte_tire.valeur = valeurs[v]; //La valeur de la carte prends la valeur tiré (noté v) dans la tableau valeurs 
       carte_tire.famille = familles[f]; //La famille prend la famille tiré (noté f) dans le tableau familles


       return carte_tire;

   }
```
Pour cela une valeur aléatoire entre 0 et 12 sera prise et correspondra à la valeur de la carte présent dans ce tableau 

-> public static char[] valeurs = { 'A', 'R', 'D', 'V', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

Même principe pour la famille sauf qu'ici c'est de 0 à 3, correspondant à coeur, carreau, trèfle et pique.

-> public static char[] familles = { '\u2665', '\u2666', '\u2663', '\u2660' };


### 2 - Carte unique 

Cette fonction (private static bool carteUnique(carte uneCarte, carte[] unJeu, int numero)) empêche d'avoir plusieurs fois la même carte dans notre main, elle vérifie si une carte existe déjà.


   ```C#
public static bool carteUnique(carte uneCarte, carte[] unJeu, int numero) 
{
    for (int i = 0; i < 5; i++) //boucle qui parcoure la main de 5 cartes 
    {

        if (i == numero) continue; //Numéro est le numéro de la carte parmi les 5 cartes de la main. Si on arrive à la position (numéro) de la carte qu'on vérifie, on passe à la suivante. Comparer la carte avec elle même donnerai un doublon.
        {
            if (uneCarte.valeur == unJeu[i].valeur && uneCarte.famille == unJeu[i].famille)
            {
                return false; //Si false la carte est unique 
            }
        

        }
               
    }
    return true; //Si true la carte existe déjà
}
```


### 3 - private static void tirageDuJeu(carte[] unJeu)

### 4 - private static void echangeDeCartes(carte[] unJeu, int[] e)

### 5 - private static combinaison chercheCombinaison(carte[] unJeu)

### 6 - private static void echangeDeCartes(carte[] unJeu, int[] e)
