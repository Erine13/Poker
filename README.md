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
                return false; //Si return false la carte existe déjà
            }
        

        }
               
    }
    return true; //Si true la carte est unique 
}
```


## 3 - Combinaisons

Cette fonction (private static combinaison chercheCombinaison(carte[] unJeu)) permet d’analyser les 5 cartes du jeu du joueur afin de déterminer la combinaison de poker obtenue (rien, paire, double paire, brelan, quinte, full, couleur, carré ou quinte flush) et de retourner cette combinaison.
La suite du programme sera composé de 8 sous parties représentant les différentes combinaisons.
Dans cette fonction, les boucles en dessous vont vérifier le nombre de similitudes entre les cartes de la main du joueur afin de remplir le tableau "similaire" :  ```  similaire = { 0, 0, 0, 0, 0 }  ```
Le tableau "similaire" représente les 5 cartes du joueur. Si la carte est unique de part sa valeur, un 1 sera mis dans le tableau au positionement de la carte. S'il y a plusieurs fois la même valeur, le tableau s'incrémentera.


 ```C#
public static combinaison cherche_combinaison(ref carte[] unJeu)
{
    int[]  similaire = { 0, 0, 0, 0, 0 };  //Tableau similaire
    int couleur = 0;
    combinaison retour = new combinaison(); //Creer une nouvelle instance de combinaison
    retour = combinaison.RIEN; //"Variable" combinaison au nom de retour
    bool brelan = false;
    bool paire = false;
    
    bool quinte = false;

 for (int i = 0; i < unJeu.Length; i++) //Comparer ma main avec ma main, i prend la premiere carte puis la 2 ... Et j compare la 1ere avec les 5 cartes.
 {
     for (int j = 0; j < unJeu.Length; j++)
     {
         if (unJeu[i].valeur == unJeu[j].valeur) //Si carte identique faire +1 dans le tableau similaire ex : brelan -> similaire = { 1, 3, 1, 3, 3 }
         {
             similaire[i] += 1;

         }

         if (unJeu[i].famille == unJeu[j].famille)
         {
             couleur += 1;
         }
     }
 }
```



### 3.1 - 

 ```C#
//Paire
if (similaire[s] == 2) //Si il y a un 2 dans similaire, faire +1 au compteur
{
    compte = compte + 1;
    paire = true;
    retour = combinaison.PAIRE;

}
```

### 4 - Echange de cartes 

private static void echangeDeCartes(carte[] unJeu, int[] e)

 ```C#
 private static void echangeCarte(ref carte[] unJeu, ref int[] e)
 {
     for (int i = 0; i < e.Length; i++) //Parcours tout le tableau e et i est l'indice, le positionnement dans e
     {
         unJeu[e[i]] = tirage(); //Change les cartes enregistrer dans le tableau e 
     }
 }
```

### 5 - Tirage du jeu

private static void tirageDuJeu(carte[] unJeu)

 ```C#
 private static void tirageDuJeu(ref carte[] unJeu)
 {
     for (int t = 0; t < 5; t++) //Tirage d'un jeu de 5 cartes 
                                 //tirage de i jusqu'à 5
     {
         do
         {
             unJeu[t] = tirage(); //UnJeu remplie le tableau tirage
         }
         while (!carteUnique(unJeu[t], unJeu, t));
     }
 }
```

### 6 - Enregistrement des cartes 


 ```C#
 using (f = new BinaryWriter(new FileStream("scores.txt", FileMode.Append, FileAccess.Write))) //append -> ajouter
 {
     f.Write(nom);
     for (int e = 0; e < 5; e++)
     {
         f.Write(MonJeu[e].valeur);
         f.Write(MonJeu[e].famille);
     }


 }
```

### 7 - Affichage des cartes


 ```C#
 using (BinaryReader f = new BinaryReader(new FileStream("scores.txt", FileMode.Open, FileAccess.Read))) 
 {
     nom = f.ReadString();
     for (int l = 0; l < 5; l++)
     {
         MonJeu[l].valeur = f.ReadChar();

         r = f.ReadChar();
         if (Char.ToString(r)== "e")
         {
             MonJeu[l].famille = '\u2665';
         }
         else if (Char.ToString(r) == "f")
         {
             MonJeu[l].famille = '\u2666';
         }
         else if (Char.ToString(r) == "c")
         {
             MonJeu[l].famille = '\u2660';
         }
         else 
         {
             MonJeu[l].famille = '\u2663';
         }
 
     }

 }
```
