using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Poker
{
    class Program
    {
        // -----------------------
        // DECLARATION DES DONNEES
        // -----------------------
        // Importation des DL (librairies de code) permettant de gérer les couleurs en mode console
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);
        static uint STD_OUTPUT_HANDLE = 0xfffffff5;
        static IntPtr hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
        // Pour utiliser la fonction C 'getchar()' : sasie d'un caractère
        [DllImport("msvcrt")]
        static extern int _getch();

        //-------------------
        // TYPES DE DONNEES
        //-------------------

        // Fin du jeu
        public static bool fin = false;

        // Codes COULEUR pour le visuel de la console
        public enum couleur { VERT = 10, ROUGE = 12, JAUNE = 14, BLANC = 15, NOIRE = 0, ROUGESURBLANC = 252, NOIRESURBLANC = 240 };

        // Coordonnées pour l'affichage
        public struct coordonnees
        {
            public int x;
            public int y;
        }

        // Une carte
        public struct carte
        {
            public char valeur;
            public int famille;
        };

        // Liste des combinaisons possibles
        public enum combinaison { RIEN, PAIRE, DOUBLE_PAIRE, BRELAN, QUINTE, FULL, COULEUR, CARRE, QUINTE_FLUSH };

        // Valeurs des cartes : As, Roi,...
        public static char[] valeurs = { 'A', 'R', 'D', 'V', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

        // Codes ASCII (3 : coeur, 4 : carreau, 5 : trèfle, 6 : pique)
        public static char[] familles = { '\u2665', '\u2666', '\u2663', '\u2660' };

        // Numéros des cartes à échanger
        public static int[] echange = { 0, 0, 0, 0 };

        // Jeu de 5 cartes
        public static carte[] MonJeu = new carte[5];

        //----------
        // FONCTIONS
        //----------

        // Génère aléatoirement une carte : {valeur;famille}
        // Retourne une expression de type "structure carte"
        public static carte tirage()
        {
            carte carte_tire;
            Random random = new Random();
            int v = random.Next(0, 13); //Prend une valeur aléatoire entre 0 et 12, correspondant à une valeur de carte présent dans la tableau 0 = A
            int f = random.Next(0, 4); //Prend une valeur aléatoire entre 0 et 3, correspondant à une famille (coeur,pique...)
            carte_tire.valeur = valeurs[v]; //La valeur de la carte prends la valeur tiré (noté v) dans la tableau valeurs 
            carte_tire.famille = familles[f]; //La famille prend la famille tiré (noté f) dans le tableau familles

            //char mava = valeurs[0];
            //char fami = familles[0];
            //ex : cartes.valeur = 'A';
            //ex : cartes.famille = '\u2665';

            return carte_tire;

        }

        // Indique si une carte est déjà présente dans le jeu
        // Paramètres : une carte, le jeu 5 cartes, le numéro de la carte dans le jeu
        // Retourne un entier (booléen)
        public static bool carteUnique(carte uneCarte, carte[] unJeu, int numero) //numéro de la carte parmi les 5 cartes de la main
        {
            for (int i = 0; i < 5; i++) //boucle de 5
            {

                if (i == numero) continue;//Si on arrive à la position (numéro) de la carte qu'on vérifie, on passe à la suivante. Comparer la carte avec elle même donnerai un doublon.
                {
                    if (uneCarte.valeur == unJeu[i].valeur && uneCarte.famille == unJeu[i].famille)
                    {
                        return false; //Si false la carte existe déjà
                    }
                

                }
                       
            }
            return true; //Si true la carte est unique 
        }

        // Calcule et retourne la COMBINAISON (paire, double-paire... , quinte-flush)
        // pour un jeu complet de 5 cartes.
        // La valeur retournée est un élement de l'énumération 'combinaison' (=constante)
        public static combinaison cherche_combinaison(ref carte[] unJeu)
        {
            int[] similaire = { 0, 0, 0, 0, 0 };  //Tableau similaire
            //int[] couleur = { 0, 0, 0, 0, 0 };
            int couleur = 0;
            combinaison retour = new combinaison(); //Creer une nouvelle instance de combinaison
            retour = combinaison.RIEN;//"Variable" combinaison au nom de retour
            bool brelan = false;
            bool paire = false;
            
            bool quinte = false;

            for (int i = 0; i < unJeu.Length; i++) //Comparer ma main avec ma main, i prend la premiere carte puis la 2 ... Et j compare la 1ere avec ses 5 cartes.
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

            char[,] quintes = { { 'X', 'V', 'D', 'R', 'A' }, { '9', 'X', 'V', 'D', 'R' }, { '8', '9', 'X', 'V', 'D' }, { '7', '8', '9', 'X', 'V' } };
            int compte = 0;

            for (int s = 0; s < similaire.Length; s++) //boucle qui regarde notre main
            {

                //Paire
                if (similaire[s] == 2) //Si il y a un 2 dans similaire, faire +1 au compteur
                {
                    compte = compte + 1;
                    paire = true;
                    retour = combinaison.PAIRE;

                }
                //Double paire
                if (compte / 2 == 2)  //Pour double paire compte = 4 (Présence de 4 fois 2 dans similaire similaire = { 2, 2, 0, 2, 2 };)
                {
                    retour = combinaison.DOUBLE_PAIRE;

                }

                //Brelan
                if (similaire[s] == 3) //similaire = { 3, 1, 3, 3, 1 };
                {

                    retour = combinaison.BRELAN;
                    brelan = true;
                }

                //Carre
                if (similaire[s] == 4)
                {
                    retour = combinaison.CARRE;
                }

                //Full
                if (paire && brelan)
                {
                    retour = combinaison.FULL;
                }

              
            }
                //Quinte
                int compteur1 = 0;
                int compteur2 = 0;
                for (int q = 0; q < similaire.Length; q++) //boucle qui regarde notre main
                {
                    if (similaire[q] == 1)//Si carte unique dans similaire = 1, ajouter + 1 au compteur
                    {
                        compteur1 += 1;//Ajoute +1 au compteur
                    }
                }
                if (compteur1 == 5) //Si le compteur est = 5 alors on verifie si la main correspond à une possibilité de quinte
                {
                        for (int m = 0; m < 4; m++) //Vérifie les petits tableau en ligne dans le grand tableau quintes 
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

           
            //Couleur
            if (couleur == 25)
            {

                retour = combinaison.COULEUR;

            }



            //Quinte Flush
            if (couleur == 25 && quinte)
            {
               retour = combinaison.QUINTE_FLUSH;
            }

            return retour;
        }

            
        

        // Echange des cartes
        // Paramètres : le tableau de 5 cartes et le tableau des numéros des cartes à échanger
        // e est un tableau ou on entre le numéro des cartes à échanger
        private static void echangeCarte(ref carte[] unJeu, ref int[] e)
        {
            for (int i = 0; i < e.Length; i++) //Parcours tout le tableau e et i est l'indice, le positionnement dans e
            {
                unJeu[e[i]] = tirage(); //Change les cartes enregistrer dans le tableau e 
            }
        }

        // Tirage d'un jeu de 5 cartes
        // Paramètre : le tableau de 5 cartes à remplir
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
        // Affiche à l'écran une carte {valeur;famille} en fournisant la colonne de départ
        private static void affichageCarte(ref carte uneCarte)
        {
            //----------------------------
            // TIRAGE D'UN JEU DE 5 CARTES
            //----------------------------
            int left = 0;
            int c = 1;
            // Tirage aléatoire de 5 cartes
            for (int i = 0; i < 5; i++)
            {
                // Tirage de la carte n°i (le jeu doit être sans doublons !)

                // Affichage de la carte
                if (MonJeu[i].famille == '\u2665' || MonJeu[i].famille == '\u2666')
                    SetConsoleTextAttribute(hConsole, 252);
                else
                    SetConsoleTextAttribute(hConsole, 240);
                Console.SetCursorPosition(left, 5);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
                Console.SetCursorPosition(left, 6);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, '|');
                Console.SetCursorPosition(left, 7);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
                Console.SetCursorPosition(left, 8);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)MonJeu[i].famille, ' ', ' ', ' ', ' ', ' ', ' ', ' ', (char)MonJeu[i].famille, '|');
                Console.SetCursorPosition(left, 9);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, ' ', ' ', ' ', '|');
                Console.SetCursorPosition(left, 10);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)MonJeu[i].famille, ' ', ' ', (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, ' ', ' ', (char)MonJeu[i].famille, '|');
                Console.SetCursorPosition(left, 11);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, (char)MonJeu[i].valeur, ' ', ' ', ' ', '|');
                Console.SetCursorPosition(left, 12);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)MonJeu[i].famille, ' ', ' ', ' ', ' ', ' ', ' ', ' ', (char)MonJeu[i].famille, '|');
                Console.SetCursorPosition(left, 13);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
                Console.SetCursorPosition(left, 14);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, ' ', (char)MonJeu[i].famille, '|');
                Console.SetCursorPosition(left, 15);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
                Console.SetCursorPosition(left, 16);
                SetConsoleTextAttribute(hConsole, 10);
                Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", ' ', ' ', ' ', ' ', ' ', c, ' ', ' ', ' ', ' ', ' ');
                left = left + 15;
                c++;
            }

        }


        //--------------------
        // Fonction PRINCIPALE
        //--------------------
        static void Main(string[] args)
        {
            //---------------
            // BOUCLE DU JEU
            //---------------
            string reponse;

            Console.OutputEncoding = Encoding.GetEncoding(65001);

            SetConsoleTextAttribute(hConsole, 012);
            while (true)
            {
                // Positionnement et affichage
                Console.Clear();
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', 'P', 'O', 'K', 'E', 'R', ' ', ' ', '|');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', '1', ' ', 'J', 'o', 'u', 'e', 'r', ' ', '|');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', '2', ' ', 'S', 'c', 'o', 'r', 'e', ' ', '|');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', '3', ' ', 'F', 'i', 'n', ' ', ' ', ' ', '|');
                Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
                Console.WriteLine();
                // Lecture du choix


                do
                {
                    SetConsoleTextAttribute(hConsole, 014);
                    Console.Write("Votre choix : ");
                    reponse = Console.ReadLine();
                }
                while (reponse != "1" && reponse != "2" && reponse != "3");
                Console.Clear();
                SetConsoleTextAttribute(hConsole, 015);
                // Jouer au Poker
                if (reponse == "1")
                {
                    int i = 0;
                    tirageDuJeu(ref MonJeu);
                    affichageCarte(ref MonJeu[i]);

                    // Nombre de carte à échanger
                    try
                    {
                        int compteur = 0;
                        SetConsoleTextAttribute(hConsole, 012);
                        Console.Write("Nombre de cartes a echanger <0-5> ? : ");
                        compteur = int.Parse(Console.ReadLine());
                        int[] e = new int[compteur];
                        for (int j = 0; j < e.Length; j++)
                        {
                            Console.Write("Carte <1-5> : ");

                            e[j] = int.Parse(Console.ReadLine());
                            e[j] -= 1;
                        }

                        echangeCarte(ref MonJeu, ref e);

                    }
                    catch { }
                    //---------------------------------------
                    // CALCUL ET AFFICHAGE DU RESULTAT DU JEU
                    //---------------------------------------
                    Console.Clear();
                    affichageCarte(ref MonJeu[i]);
                    SetConsoleTextAttribute(hConsole, 012);
                    Console.Write("RESULTAT - Vous avez : ");
                    try
                    {
                        // Test de la combinaison
                        switch (cherche_combinaison(ref MonJeu))
                        {
                            case combinaison.RIEN:
                                Console.WriteLine("rien du tout... desole!"); break;
                            case combinaison.PAIRE:
                                Console.WriteLine("une simple paire..."); break;
                            case combinaison.DOUBLE_PAIRE:
                                Console.WriteLine("une double paire; on peut esperer..."); break;
                            case combinaison.BRELAN:
                                Console.WriteLine("un brelan; pas mal..."); break;
                            case combinaison.QUINTE:
                                Console.WriteLine("une quinte; bien!"); break;
                            case combinaison.FULL:
                                Console.WriteLine("un full; ouahh!"); break;
                            case combinaison.COULEUR:
                                Console.WriteLine("une couleur; bravo!"); break;
                            case combinaison.CARRE:
                                Console.WriteLine("un carre; champion!"); break;
                            case combinaison.QUINTE_FLUSH:
                                Console.WriteLine("une quinte-flush; royal!"); break;
                        }
                        ;
                    }
                    catch { }
                    Console.ReadKey();
                    char enregister = ' ';
                    string nom = "";
                    BinaryWriter f; //Variable FICHIER, permet de stocker
                    SetConsoleTextAttribute(hConsole, 014);
                    Console.Write("Enregistrer le Jeu ? (O/N) : ");
                    enregister = char.Parse(Console.ReadLine());
                    enregister = Char.ToUpper(enregister);

                    if (enregister == 'O')
                    {
                        const string fileName = "scores.txt";
                        Console.WriteLine("Vous pouvez saisir votre nom (ou pseudo) : ");
                        nom = Console.ReadLine();
                        //BinaryWriter f; Variable fichier
                        //Ouverture du fichier en AJOUT
                        //Si le fichier EXISTE : ajout à la fin sinon création du fichier
                        using (f = new BinaryWriter(new FileStream("scores.txt", FileMode.Create, FileAccess.Write))) //append -> ajouter à la suite   Create -> Ecrit à la place 
                        {
                            f.Write(nom);
                            for (int e = 0; e < 5; e++)
                            {
                                f.Write(MonJeu[e].valeur);
                                f.Write(MonJeu[e].famille);
                            }


                        }

                    }

                }
                if (reponse == "2")
                {
                    string articles; //Article à écrire dans le fichier
                    char[] délimiteurs = { ';' }; //Caractères délimiteurs, couper chaine de caractère
                    carte UneCarte; //Une carte
                    string nom; //Nom du joueur
                    Array r1; // Array = un tableau
                    char r;
                    if (File.Exists("scores.txt"))
                    {
                        //Ouverture en LECTURE
                        using (BinaryReader f = new BinaryReader(new FileStream("scores.txt", FileMode.Open, FileAccess.Read))) 
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
                        
                        Console.WriteLine("Nom : " + nom);
                        affichageCarte(ref MonJeu[0]); //ref fait référence à mon jeu du dessus
                        Console.ReadKey();
                    }
                }

                if (reponse == "3")
                    break;

            }
            Console.Clear();
            Console.ReadKey();
        }
    }
}

