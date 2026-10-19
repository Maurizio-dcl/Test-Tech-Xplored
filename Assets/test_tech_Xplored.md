# Note generali

I giochi sono realizzati in Unity, 2D, all'interno del Canvas della UI.
I giochi possono essere giocati con un solo dispositivo o possono essere usati dispositivi
aggiuntivi connessi via rete per controllare uno o più personaggi per dispositivo. In questo caso un
dispositivo centrale comunica nel suo schermo le informazioni principali sul gioco, mentre sui
dispositivi secondari vi osno i controlli per i relativi personaggi. Il dispositivo principale
mostra la mappa, le informazioni relative alla fase di gioco, alla turnazione, al party.
Il dispositivo principale è poco o niente interattivo durante la partita. Viene usato per crearla,
mentre gli altri dispositivi si uniscono alla partita creata. La rete può funzionare sia con
internet che con la rete locale, contemporaneamente.
I giochi sono da considerarsi phygital, con sempre una controparte di pezzi fisici che i giocatori
posizionano e manipolano sul tavolo.
Alcune parti delle specifiche potrebbero essere in inglese.
Le specifiche potrebbero non essere complete o potrebbero contraddirsi.
Chiedere per qualsiasi dubbio sulle specifiche.

# Giochi

## To Be Killed
Su una griglia 8x5 Si muovono dei personaggi, controllati dal giocatore. Il movimento è solo
Manhattan.
Il gioco è a turni. Ogni turno un personaggio può muovere e poi agire.
Il movimento che un personaggio può effettuare in un turno è di due passi.
I personaggi sono 4 (A,B,C,D), ognuno con un set di azioni differenti. Tutti i personaggi hanno
un'azione comune chiamata "Focus" che da un +2 al prossimo tiro di dado.
Le azioni hanno un cooldown in numero di turni. Cooldown 0 vuol dire che si può usare sempre.

Cooldown 1 vuol dire che si può usare un turno sì e uno no.

 - A.
   1. Attacco a distanza di massimo 2 di 2 danni più 1d6 `[aggro danni - 2][cooldown 1]`
   2. Attacco melee da 3 danni più 1d6 `[aggro danni][cooldown 0]`

 - B.
   1. Cura sé di 10HP `[aggro 1][cooldown 3]`
   2. Attacco 4 danni più 1d6 `[aggro danni + 2][cooldown 1]`

 - C.
   1. Attacco di 1 danno più 1d6 più un danno per ogni alleato adiacente `[aggro danni -1][cooldown 1]`
   2. Cura tutti gli alleati adiacenti di 2 + 1d6 `[aggro pari al numero di alleati curati * 2][cooldown 3]`

 - D.
   1. Sposta un nemico di 1d6 / 2 (per difetto) `[aggro 6][cooldown 2]`
   2. Sposta un alleato di 1d6 `[aggro distanza mossa * 3][cooldown 2]`

Ogni personaggio ha tot HP:
 - A. 20
 - B. 30
 - C. 15
 - D. 10

L'unico nemico è un boss.
Il boss ha 50HP.
Il boss ha un livello di aggro per ogni personaggio che determina quale sia il suo target.
Ogni azione dei personaggi aggiunge un valore di aggro al loro stack per il boss.
Ad ogni inizio turno del boss il valore di aggro di ogni personaggio diminuisce di uno. Ad ogni
inizio turno di ogni giocatore il proprio valore di aggro diminuisce di uno.
Il bersaglio del boss è il personaggio con più aggro.
Ad ogni suo turno il boss guarda se può usare delle azioni contro il suo bersaglio senza muoversi.
Se sì ne usa una disponibile random.
Se no si muove verso il suo bersaglio, e usa una delle azioni disponibili dopo essersi mosso.
Se non ha nessuna azione disponibile contro il suo bersaglio controlla se ha delle azioni
disponibili contro il bersaglio più vicino, spareggiando per valore di aggro prima e a caso in caso
di altro pareggio, e la usa. Potrebbe non poter usare alcuna azione, nel caso non ne usa.

Il boss può passare attraverso i personaggi.
I personaggi non possono passare attraverso il boss, ma possono passare attraverso i personaggi.

Il boss può muovere di 3.

Le azioni del boss sono:

 1. Attacco melee a tutti attorno a sé 5 + 1d6 `[cooldown 3]`
 2. Attacco a distanza da minimo 4 di distanza per 3d3 `[cooldown 1]`
 3. Attacco melee 2+1d6 `[cooldown 1]`
 4. Attacco melee 2 danni push target a distanza 2 `[cooldown 1]`
 5. [Se non ha nessun aggro] Cura sé di 15HP `[Cooldown 1]`

## Armed & Adventurous

Dungeon crawler. La mappa di gioco e composta da tile. Le tile hanno fronte e retro diversi. Ogni
tile è divisa in zone. Ci può essere anche solo una zona. Le zone sono a volte separate da muri.
Le zone coprono almeno uno dei vertici di una griglia 8x5, con un bordo di 0.5, ovvero vi è spazio
per una zona di uno di lato centrata su ogni vertice. Le zone non devono essere allineate e possono
coprire anche più vertici.
I muri bloccano la linea di vista, che viene calcolata da un punto all'interno di ogni zona.
Le tile e le zone devono poter aver forme arbitrarie. I muri devono poter essere bucati a runtime,
e aggiunti a runtime, da token che il giocatore mette sopra per avere un feedback visivo, come
istruito dal gioco. Il movimento avviene sul grafo delle zone. Alcune zone hanno delle proprietà
che influiscono sul costo di movimento, e.g. entrare costa +1, uscire da +1 movimento.
Le tile hanno delle aperture che permettono di collegarle tra loro se affiancate. Di queste aperture
non tutte devono essere posizionate esattamente affiancate, ma alcune possono essere più larghe e
accettare aperture disallineate.

Nel dungeon spawnano mostri ogni volta che si apre una porta e in seguito ad alcune interazioni.
Le porte sono sempre sulla soglia tra due tile. Aprire una porta attiva il gruppo di tile oltre di
essa. Ogni turno un sub set dei mostri viene attivato, in base ad un random sulle caratteristiche
dei mostri presenti. E.g. tutti gli umani, tutti i goblin, tutti i mostri volanti, tutti i mostri
feriti, tutti i mostri con la miniatura blu, tutti i mostri con la miniatura verde, tutti i boss,
ecc.
I mostri che spawnano sono estratti da un set di mostri in maniera da arrivare ad un totale di punti
in base al punteggio assegnato al gruppo di zone, scalato per il power level del gruppo di
giocatori. I mostri in gioco non possono mai superare il numero per ogni tipo impostato globalmente
per il gioco. E.g. 10 goblin, 5 ladri, 1 golem di fuoco, 1 viverna, 2 arci cultisti, 8 cultisti.
L'IA dei mostri è un flow chart con al massimo 3 check, con condizioni e set di azioni.

Le azioni possibili sono:
 1. movimento verso l'eroe più vicino
 2. movimento verso l'eroe con meno hp
 3. movimento verso l'eroe con più ferite
 4. movimento verso l'healer
 5. attacca l'eroe più vicino
 6. attacca l'eroe più lontano
 7. attacca l'eroe con più hp
 8. attacca l'eroe con più armatura
 9. lancia l'eroe più vicino di X spazi
 10. lancia l'eroe più vicino nella zona più lontana dai suoi alleati entro distanza X da sé
 11. cura l'alleato più vicino
 12. cura l'alleato con più danni
 13. cura l'alleato con meno hp
 14. lancia magia nella propria zona
 15. lancia magia aoe
 16. lancia magia nella zona con più nemici

I check possibili sono:
1. se ha un nemico nella stessa zona
2. se ha un nemico entro X
3. se è ferito
4. se ha meno di X hp
5. se ha almeno un alleato ferito nella stessa zona
6. se ha almeno un alleato ferito entro X
7. se ha almeno N alleti feriti entro X
8. se ha almeno un alleato con meno del 10% dei suoi hp
9. se è avvelenato
10. se sta bruciando

I personaggi dei giocatori hanno un ammontare di punti azione ogni turno, e un ammontare di punti
movimento. Alcuni hanno anche un ammontare di punti magia. I punti magia non sono resettati ad ogni
turno ma ogni round viene riempida di 1/3 del totale, senza sforare il massimo.
I giocatori scelgono ad ogni turno chi agirà dei loro personaggi che ancora deve agire. I personaggi
hanno a disposizione un set di azioni che hanno un costo, delle condizioni di utilizzo, e una lista
di effetti. Ogni azione ha un cooldown. Cooldown 1 vuol dire che la si può giocare ogni turno.
Cooldown 2 che la si può giocare un turno sì e uno no.

I personaggi hanno degli oggetti equipaggiati, come elmo, armatura, pantaloni, calzature, guanti,
anelli, occhiali, armi a distanza, armi melee, armi ad una mano, armi a due mani, ecc.
Gli slot di equipaggiamento sono:
1. testa
2. busto
3. mani
4. gambe
5. piedi
Le armi concorrono nell'uso di slot per utensili di due mani. Gli oggetti indossabili come armature,
anelli, copricapi, concorrono su un altro layer di slot.

Ogni oggetto può avere una limitazione data dalla tipologia di classe e di corporatura.
Ad esempio classe guerriero, classe mago, corporatura robusta, corporatura minuta, corporatura
media.

Gli oggetti possono essere lasciati per terra e possono essere raccolti da terra. Farlo costa un
punto azione e si possono spostare tutti gli oggetti che si vuole durante quell'azione.
L'azione di cercare in una zona rivela alcuni oggetti, rendendoli disponibili per essere raccolti da
terra. Quali e quanti oggetti vengono rivelati è dato da una prova al dado in base alla
caratteristica di percezione.

Oltre agli oggetti i giocatori hanno anche un ammontare di denaro, comune. Per terra si può
accumulare del denaro, espresso in un unico valore.

Quando un mostro muore lascia a terra le proprie armi e in seguito ad azioni di ricerca possono
comparire altri oggetti e denaro.

I personaggi hanno delle caratteristiche usate per prove e per potenziare le azioni.
Le caratteristiche sono:
1. Arcana
2. Percezione
3. Resistenza
4. Forza
5. Agilità

Ogni mostro ucciso da un ammontare di punti esperienza al party. Ad ogni fine turno i personaggi
possono spendere i punti esperienza del party per aumentare il proprio livello. Ogni livello costa
il numero di punti esperienza pari al livello da raggiungere al quadrato. Ogni livello multiplo di 5
vengono aumentati i punti azione di uno. Ogni altro livello si può aumentare di uno una delle
caratteristiche con valore più basso. Per aumentare una caratteristica che non è la più bassa
bisogna spendere un ammontare di punti esperienza pari al livello da raggiungere, con incrementi di
uno.

I personaggi giocanti sono 4 o 5 alla volta. Sono scelti tra un pool di 20 personaggi.

L'obiettivo della partita è uno dei seguenti:
1. portare un certo oggetto in una certa zona lasciandolo per terra
2. uccidere il boss
3. resistere X turni
4. uccidere tutti

Se un personaggio muore respawna nell'ultimo checkpoint al turno successivo. I checkpoint sono token
che offrono l'azione contestuale di attivarlo. I personaggi spawnano con il 20% in meno del loro
massimale di hp, a vita piena, mana pieno, con i cooldown resettati.

## To Be Killed : Expansions

In these expansions for To Be Killed we add 10 new bosses, 20 new characters, and new mechanics.
Each boss has its own map. Maps are a single piece. On the backside there is another map. On the
original boss there was nothing.
All actions of the characters can be leveled up during a game performing actions. At the start of
the game the actions start from level 1, except for some that during the campaign are permanently
leveled up. Actions could be leveled by moving a certain amount of distance, dealing a certain
amount of damages to the boss, healing a certain amount of hps, taking a certain amount of damage.
Characters have a characteristic: humanity. Certain actions increase or decrease their level. Some
actions of the bosses decrease their level. They start at 10 and when they reach 0 they become
Inhuman. The actions of the characters use the humanity level to change ther outcome. When a
character is inhuman all the cards change and have a slightly different effect. Generally more
powerful but also self harming and harming the team. Some become more volatile, with more out
random.

# Servizi

## Cloud Saves

Saves are stored per profile on the devices and can be synced with the cloud if a profile is an
online profile and if the cloud is reachable (just a backend really). Saves conflicts must be merged
automatically by the games where possible and the user should be prompted in the other cases.

## Analytics

Ad ogni movimento e azione viene loggata la cosa nelle analytics. Quale personaggio o il boss muov
eda dove a dove e quale azione compie, con che valore di dado tirato e con che aggro risultante.
Viene loggato anche l'evento di diminuzione di aggro.
Ogni log di analytics ha un timestamp e salva alcuni dati relativi alla macchina (pc o device
mobile).
Questi dati vengono mandati ad un servizio web tramite richieste https, autenticandosi con una
chiave che identifica anche il gioco.
I dati vengono salvati in un database mongodb.
Il servizio web può essere implementato in go o in nextjs.

Analytics ha una pagina di visualizzazione che mostra un aggregato dei dati e permette di isolare le
singole partite e vederne i log.
La visualizzazione mostra sia un grafico di HP per turno per ogni personaggio e boss, che la loro
posizione su griglia.
L'aggregato mostra la durata media dei turni, quanti game sono finiti in vittoria e quanti in
sconfitta e la durata media e mediana delle sconfitte e delle vittorie. Mostra un grafico per turno
con gli hp medi e mediani di ogni personaggio e del boss. Scegliendo un turno mostra una heatmap
delle posizioni del boss e di ogni personaggio.

I servizi di analytics devono essere deployati tramite docker e tramite portainer, che possono
essere installati in locale.