![Logo](https://i.ibb.co/QDJdpPx/image.png)

# Projet 2CP -Archimède-
Archimède est un outil d'optimisation de fonctions logiques  par des méthodes algorithmiques (notamment des améliorations de Quinn McCluskey), et synthèse de la fonction simplifiée.

## Utilisé par

Ce projet est utilisé par :
- les étudiants du cycle préparatoire du domaine de l'informatique.
- l'ESI également.

## Auteurs

* [AMEUR Nassim](https://github.com/NassimAm)
* [BENBETKA Marouane](https://github.com/MarouaneBenbetka)
* [ZEMMOURI Fayssal](https://github.com/FaycalZM)
* [BESSALAH Aniss](https://github.com/anissbslh)
* [ASSELAH Wahid](https://github.com/wahidaslh)
* [MENNAD Hania](https://github.com/haniamennad1)

## Charte graphique

| Couleur             | Code Hexadécimal                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Bleu-sarcelle | ![#00CBBD](https://via.placeholder.com/10/00cbbd?text=+) #00CBBD |
| Vert clair | ![#00E17C](https://via.placeholder.com/10/00E17C?text=+) #00E17C |
| Notion noire | ![#002725](https://via.placeholder.com/10/002725?text=+) #002725 |
| Notion blanche | ![#FFFFFF](https://via.placeholder.com/10/FFFFFF?text=+) #FFFFFF |


## Documentation

* [Playlist WPF Frameworks](https://www.youtube.com/playlist?list=PLrW43fNmjaQVYF4zgsD0oL9Iv6u23PI6M)
* [Formation C#](https://www.w3schools.com/cs/index.php)
* [Site officiel Microsoft Docs](https://docs.microsoft.com/en-us/)
* [Algorithme Espresso](https://www.youtube.com/watch?v=isqtQmWpDtg)
* [Documentation Quinn McCluskey](http://myreader.toile-libre.org/Documentation_QMC.pdf)
* [Documentation sur la minimisation des fonctions logiques](http://myreader.toile-libre.org/Documentation_minimisation.pdf)
* [Documentation sur l'optimisation des circuits combinatoires](http://myreader.toile-libre.org/Documentation_optimisation.pdf)
* [Documentation sur l'optimisation des circuits combinatoires II](http://myreader.toile-libre.org/optimisation_II.pdf)

## Guide d'utilisation
 - ***Bienvenue :***
Au lancement du logiciel, la première fenêtre qui vous est affichée est la suivante :

![App Screenshot](https://i.ibb.co/Rcyy4yb/cran-d-acceuil.png)

Cliquez sur le boutton "Commencer" afin d'accéder à l'écran de l'introduction de la fonction

![App Screenshot](https://i.ibb.co/DRB7fpH/Page-d-acceuil.png)

Vous pouvez également visiter le site web -Archimède- en cliquant sur ce qui est indiqué.

![App Screenshot](https://i.ibb.co/KLQwK6V/Page-d-acceuil.png)

 - ***Introduction de la fonction :***

Comme il est clair ci-dessus ; vous avez deux choix : 
- *forme littérale :*
![App Screenshot](https://i.ibb.co/7krBCWc/Entr-e-de-fonction.png)
- *forme numérique :*
![App Screenshot](https://i.ibb.co/vB8f9J6/Entr-e-de-fonction.png)

L'écran est également doté d'un menu qui liste les fonctionnalités suivantes :

* une option de retour à l'écran d'acceuil.
* accéder à la documentation utilisée dans la conception du progiciel.
* visiter le site web d'aide en cas de besoin.

Vous pouvez consulter ce dernier en cliquant sur l'icone encadrée ci-dessous :

![App Screenshot](https://i.ibb.co/yP6QZZ6/Entr-e-de-fonction.png)

![App Screenshot](https://i.ibb.co/YPDMsH0/Menu.png)

**1)** Afin d'introduire la fonction booléenne -qui sera considérée comme but-, l'utilisateur entre les littéraux dans le champs, où le compilateur va détécter toutes les erreurs afin de fournir une fonction correcte comme entrée dans l'execution des opérations que le progiciel propose.

**2)** Afin de faciliter la tache, un clavier a été fourni avec les sept portes logiques essentielles, ainsi que les parenthèses (particulièrement noté comme un opérateur important)

**3)** Eclairci dans l'image ultérieurement, vous avez trois bouttons illustrant les trois opérations que l'usager peut executer :

**-** La transformation,

**-** La simplification,

**-** La synthèse.


![App Screenshot](https://i.ibb.co/5vd9QnY/Entr-e-de-fonction.png)

 - ***Transformation :***
Si vous cliquez sur le boutton indiquant l'execution de l'opération de transformation, un pop-up vous donnera le choix entre 4 options que l'application suggère :

***-*** Transformer la fonction booléenne entrée en forme conjonctive. 

***-*** Transformer la fonction booléenne entrée en forme disjonctive.

***-*** Transformer la fonction booléenne entrée en une fonction utilisant des opérateurs *NAND* seulement.

***-*** Transformer la fonction booléenne entrée en une fonction utilisant des opérateurs *NOR* seulement.

![App Screenshot](https://i.ibb.co/pZJnXxP/Pop-up-transformation.png)

***-*** Paramètres fixées, l'expression tronsformée en sortie sera affichée à l'écran
![App Screenshot](https://i.ibb.co/Rjtmtt7/conjonctive-transformation.png)

![App Screenshot](https://i.ibb.co/bJpwTsX/disjonctive-transformation.png)

![App Screenshot](https://i.ibb.co/jR91K6C/nand-transformation.png)

![App Screenshot](https://i.ibb.co/GsGTxmj/nor-transformation.png)

- ***Simplification :***
Une fois le boutton avec le libbelé "Simplification" est cliqué, un autre pop-up s'affiche offrant les possibilités d'affichage du résultat suivantes :

***-*** en forme conjonctive.

***-*** en forme disjonctive.

***-*** en utilisant des opérateurs *NAND* seulement.

***-*** en utilisant des opérateurs *NOR* seulement.

Ainsi que la possibilité d'afficher les étapes détaillées de la simplification de la fonction logique mise en entrée.

***+*** l'utilisateur a également le choix de retourner vers la page ou étape précédente à chaque moment.

![App Screenshot](https://i.ibb.co/5jsPcKH/Pop-up-simplification.png)

- ***Synthèse :***

Le boutton "Synthèse" permet d'afficher un circuit logique configurable d'une fonction logique quelconque en insérant les paramètres dans le pop-up visualisé :

***-*** Nombre de portes *ET*.

***-*** Nombre de portes *OU*.

***-*** Nombre de portes *NAND*.

***-*** Nombre de portes *NOR*.

![App Screenshot](https://i.ibb.co/JtwFDPW/Pop-up-syntexe.png)

Cliquant sur "Aller", le circuit s'affichera en proposant une option de sauvegarde du circuit en format PNG.

![App Screenshot](https://i.ibb.co/jWJ1zJ1/cran-synth-se.png)

##  Merci

* Merci, cher utilisateur, de nous faire confiance. Nous espérons que cela donnerait un coup de main à quiconque en aurait besoin.
