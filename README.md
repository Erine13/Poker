# Poker

## Introduction 
Ce projet a été donnée durant une première année de BTS SIO. Le contexte de ce projet est que la société HsH souhaite offrir un espace de détente proposant de petits jeux sur l'ordinateur mais certains jeux marchent mal. Nous avons du assurer la maintenance corrective d'un programme s’inspirant du jeu du Poker. 

Le poker est un jeu où un tirage aléatoire de 5 cartes d'un jeu de 52 cartes est effectué. Le joueur devra avoir la plus haute combinaison possible avec sa main. Pour cela il aura la possibilité, soit de conserver son jeu, soit d’échanger aux plus quatre cartes, et obtenir ainsi une nouvelle main. Il pourra également enregistrer son nom avec son score.

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



### 3.1 - Paire

Combinaison PAIRE s'il y a un 2 dans le tableau similaire 
Exemple : paire -> similaire = { 1, 2, 1, 2, 1 }

 ```C#
if (similaire[s] == 2) //Si il y a un 2 dans similaire, faire +1 au compteur
{
    compte = compte + 1;
    paire = true;
    retour = combinaison.PAIRE;

}
```
Ainsi qu'un compteur qui à chaque fois que l’on a une paire (un 2 dans "similaire") on incrémente "compte", qui sera utile pour la double paire.

### 3.2 - Double paire

Pour double paire compte sera égale à 4 (Présence de 4 fois 2 dans similaire similaire = { 2, 2, 1, 2, 2 })
Si "compte" divisé par 2 sera égale à 2, la condition DOUBLE_PAIRE sera remplie.

 ```C#
if (compte / 2 == 2)  
{
    retour = combinaison.DOUBLE_PAIRE;

}
 ```


### 3.3 - Brelan

S'il y a la présence d'un 3 dans "similaire", la condition BRELAN sera remplie.
Exemple : brelan -> similaire = { 3, 1, 3, 3, 1 };


 ```C#
if (similaire[s] == 3) 
{
    retour = combinaison.BRELAN;
    brelan = true;
}
 ```


### 3.4 - Carre

S'il y a la présence d'un 4 dans "similaire", la condition CARRE sera remplie.
Exemple : carre -> similaire = { 4, 1, 4, 4, 4 };

 ```C#
if (similaire[s] == 4)
{
    retour = combinaison.CARRE;
}
 ```

### 3.5 - Full

La condition FULL est remplie lorsque la variable paire ET la variable brelan sont toutes les deux égales à true. 

 ```C#
if (paire && brelan)
{
    retour = combinaison.FULL;
}
 ```


### 3.6 - Quinte

La condition de la quinte consiste à vérifier si les cinq cartes de la main ont des valeurs différentes et si elles forment une suite de valeurs consécutives. Pour cela, le programme commence par vérifier qu’il n’y a aucun doublon dans la main, si la carte est unique, le programme fait + 1 au compteur.

Ensuite, si le compteur est égale à 5 on verifie si la main correspond à une possibilité de quinte, ces suites possibles sont stocké dans un tableau segmenté en 4 plus petits tableaux. 
Puis avec un deuxième compteur (compteur2), on regarde si les valeur de notre jeu correspondent aux valeurs présentent dans une des quintes stockées. Si c'est le cas + 1 au compteur2.
Si les cinq cartes correspondent à l’une de ces suites et donc que compteur2 = 5, la combinaison Quinte est remplie.

 ```C#
int compteur1 = 0;
int compteur2 = 0;
for (int q = 0; q < similaire.Length; q++) //boucle qui regarde notre main
{
    if (similaire[q] == 1)//Si carte unique dans similaire = 1
    {
        compteur1 += 1;//Ajoute +1 au compteur
    }
}
if (compteur1 == 5) //Si le compteur est = 5 alors on verifie si la main correspond à une possibilité de quinte
{
        for (int m = 0; m < 4; m++) //Vérifie les petits tableau en ligne dans le grand tableau quinte 
        {
            
            for (int n = 0; n < 5; n++) //Parcours notre main de 5 cartes
            {
                for (int p = 0; p < 5; p++)//Vérifie les cartes en details dans chaque petit tableau
                {

                    if (unJeu[n].valeur == quintes[m, p]) //Si valeur de notre jeu correspond au valeur présente dans une des quintes stocké, + 1 au compteur2
                    {
                        compteur2 += 1;
                        if (compteur2 == 5) //Si compteur 2 est = 5, cela correspond à une quinte
                        {
                           retour = combinaison.QUINTE;
                           quinte = true;
                        }
                    }   
                }
            }   
          compteur2 = 0;
        }
}
 ```


### 3.7 - Couleur

Si le compteur "couleur" est égale à 25, la condition COULEUR sera remplie.
Ce compteur est incrémenté quand dans la main, il y a des cartes de la même famille. 

 ```C#
if (couleur == 25)
{
    retour = combinaison.COULEUR;
}
 ```



### 3.8 - Quinte flush

La condition QUINTE_FLUSH est remplie lorsque la variable quinte est égale à true et que le compteur couleur est égale à 25.

 ```C#
 if (couleur == 25 && quinte)
 {
    retour = combinaison.QUINTE_FLUSH;
 }
 ```


### 4 - Echange de cartes 

Cette fonction (private static void echangeDeCartes(carte[] unJeu, int[] e))  permet d’échanger certaines cartes de la main du joueur. Elle parcourt le tableau contenant les positions des cartes à remplacer et génère pour chacune d’elles une nouvelle carte aléatoire grâce à la fonction "tirage()" (expliquer au début du Readme). Puis changera les cartes enregistrer dans le tableau.

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

Cette fonction permet (private static void tirageDuJeu(carte[] unJeu)) de faire le tirage du jeu / de la main de 5 cartes aléatoires. 
Pour chaque position des cartes en jeu, une carte est générée avec la fonction "tirage()". 
Puis la boucle while permet de continuer le tirage tant que la carte n'est pas unique.


 ```C#
 private static void tirageDuJeu(ref carte[] unJeu)
 {
     for (int t = 0; t < 5; t++) //Tirage d'un jeu de 5 cartes 
                                 //tirage de i jusqu'à 5
     {
         do //faire
         {
             unJeu[t] = tirage(); //UnJeu remplie par fonction tirage
         }
         while (!carteUnique(unJeu[t], unJeu, t)); //Tant que la carte n'est pas unique continue le tirage pour éviter d'avoir des cartes en doubles
     }
 }
```

### 6 - Enregistrement du résultat 

Cette fonction sert à enregistrer le nom du joueur avec  ```f.Write(nom) ``` et enregistrer les 5 cartes de la main.

 ```C#
 using (f = new BinaryWriter(new FileStream("scores.txt", FileMode.Create, FileAccess.Write))) //append -> ajouter à la suite   Create -> Ecrit à la place 
 {
     f.Write(nom);
     for (int e = 0; e < 5; e++)
     {
         f.Write(MonJeu[e].valeur);
         f.Write(MonJeu[e].famille);
     }
 }
```

### 7 - Affichage du résultat

Cette fonction sert à récupérer / lire le nom du joueur avec  ```nom = f.ReadString() ``` et lire les 5 cartes de la main.

 ```C#
 using (BinaryReader f = new BinaryReader(new FileStream("scores.txt", FileMode.Open, FileAccess.Read))) //Ouverture fichier score 
 {
     //récupération/lecture du nom du joueur dans le fichier score
     nom = f.ReadString();
     for (int l = 0; l < 5; l++) //Boucle pour parcourir les 5 cartes
     {
         MonJeu[l].valeur = f.ReadChar(); //Récupération de la valeur de la carte

         //Récupération de la famille de la carte en la comparant avec les possibilités en dessous
         r = f.ReadChar(); 
         // Conversion du caractère en symbole de carte (coeur,...) correspondant
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
         //ReadChar pour passer les éléments inutile présent dans le fichier score (sinon donne un mauvais affichage des cartes avec d'autres caractère à la place des symboles)
         r1 = f.ReadChars(3);
     }

 }
```
