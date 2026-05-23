# Double Side
## Game Design Document — v0.1 (Pre-Produzione)

---

## 1. Panoramica del progetto

**Titolo di lavoro:** Double Side
**Genere:** Metroidvania 2D a scorrimento orizzontale
**Ambientazione:** Sci-fi futuristico post-collassato
**Stile visivo:** Vector art ispirato a Hollow Knight
**Durata target:** 6-10 ore di gameplay (main path)
**Sviluppo:** Solo dev
**Stato:** Pre-produzione, brainstorming concettuale completato

### High Concept

Un costrutto si risveglia nelle rovine di una mega-città verticale costruita su una doppia struttura architettonica. La sua abilità di phase-shift gli permette di ruotare di 180° tra il "Lato A" (la facciata pubblica della città) e il "Lato B" (il retro funzionale che la sosteneva), esplorando uno stesso volume fisico da due prospettive opposte. Mentre risale dalla base verso la sommità della città scopre cosa è successo alla civiltà che l'ha creato e affronta una scelta su cosa farne.

### Unique Selling Points

- **Meccanica del flip 180°**: due lati dello stesso livello fisico, non due mondi paralleli. Coerenza spaziale come pilastro di design.
- **Telecamere diegetiche** come sistema di "sbirciata": l'informazione è una risorsa di level design.
- **Identità di costrutto** con sistema di impianti a slot fissi e intercambiabili.
- **Scelta morale a tre vie** che riconfigura il finale.
- **Antagonista che cresce col giocatore**: il Primo, un costrutto avanzato che può flippare senza restrizioni.

---

## 2. Meccanica principale — Il Flip

### Concetto

Il giocatore può ruotare la propria prospettiva di 180° in qualsiasi momento (con vincoli), passando dal **Lato A** al **Lato B** dello stesso spazio 3D. Non sono due dimensioni o due mondi: sono lo stesso livello visto da davanti o da dietro. I nemici occupano posizioni fisse nel volume e capita che siano davanti o dietro a seconda del piano su cui ti trovi.

### Regole tecniche

- **Coordinate globali preservate**: la posizione del giocatore nel mondo resta invariata dopo il flip. Cambia solo il piano (asse Z) e il punto di vista.
- **Movimento world-relative**: premere "destra" muove sempre il personaggio nella stessa direzione del mondo. Dopo il flip, questa direzione corrisponde alla sinistra dello schermo.
- **Animazione di transizione**: il flip è un'azione pesata (durata indicativa: 0.4-0.6 secondi) con anticipazione, rotazione visibile, e suono caratteristico. Non è un teleport istantaneo. L'animazione dà tempo al giocatore di riorientarsi.
- **Restrizione del flip**: il flip è impedito se la posizione del giocatore sul lato opposto sarebbe occupata da un ostacolo solido.
- **Indicatore visivo**: necessario un feedback chiaro sullo stato del flip — silhouette fantasma del giocatore sul lato opposto, o icona di flip che cambia colore quando bloccata.

### Sbirciare il Lato B

Sistema di **telecamere di sicurezza** diegetiche:

- Telecamere fisse nel mondo, ognuna con una zona di copertura definita.
- Accessibili tramite **terminali** distribuiti negli ambienti.
- Mostrano il feed in real-time del lato opposto nella zona coperta.
- Possono essere danneggiate, disabilitate, o riattivate.
- Creano zone di "piena visione" (pianificazione strategica) e zone di "cieco" (rischio + esplorazione).

### Conseguenze di design

- I nemici possono trovarsi su entrambi i lati: la sbirciata serve a capire se una via è più sicura dell'altra.
- Il level design diventa un puzzle di posizionamento: dove posso flippare? cosa c'è dietro? dove devo arrivare per poter flippare?
- Possibilità di routing alternativo: ogni stanza ha potenzialmente due vie.

---

## 3. Combat

### Approccio

Combat **melee primario** ispirato a Hollow Knight: skill-based, basato su timing, posizionamento, lettura dei pattern nemici.

### Verbs disponibili

- **Colpo base**: combo a 2-3 colpi con arma a energia (lama, baton elettrico o monofilamento — da definire).
- **Caricato**: tieni premuto per colpo più potente. Acquisibile presto.
- **Dash**: scatto breve. Inizialmente senza i-frame. Upgrade nel corso del gioco (con i-frame, attraversa nemici, ecc.).
- **Abilità da impianto**: slot riempibili. Consumano la risorsa "Cariche". Esempi: colpo a distanza, EMP, breve invisibilità, hack rapido.
- **Auto-riparazione (heal)**: consuma Cariche. Animazione lunga, ti rende vulnerabile durante il processo. Equivalente del Focus di HK.

### Risorsa — Cariche

- Si accumulano colpendo i nemici.
- Servono per healing e per attivare abilità da impianto.
- Pool massimo aumentabile tramite upgrade.

### Flip in combat

Il flip è disponibile **solo in punti specifici designati dal level design**. Non è un panic-button generalizzato. Il giocatore impara a leggere le arene cercando opportunità di flip.

Tre tipi di flip point in combat:

- **Fuga**: aperture lontane per evitare lo scontro o ritirarsi.
- **Flanking**: per attaccare da dietro nemici corazzati frontalmente.
- **Riposizionamento**: per cambiare la geometria della stanza nel mezzo dello scontro.

### Boss arena dinamiche

- Il boss apre/chiude varchi durante il combattimento (movimenti, attacchi che modificano la struttura, distruzione mirata).
- Le sue fasi corrispondono a set di flip point diversi.
- Il giocatore deve riadattare la strategia ad ogni fase.

---

## 4. Sistema impianti

### Struttura

- **6 slot iniziali**, espandibili fino a **10-12** tramite upgrade.
- **2 slot fissi** (build identity).
- **4 slot intercambiabili** (modificatori tattici).
- Gli impianti fissi possono essere cambiati **solo agli hub**, idealmente al primo ingresso di un nuovo biome.

### Slot fissi — assi di identità

- **Asse Combat**: offensivo / difensivo / agile.
- **Asse Approccio**: diretto / hacker / stealth.

La combinazione dei due assi crea archetipi di build identificabili (es. offensivo-stealth, difensivo-hacker, ecc.).

### Slot intercambiabili — esempi

- +Cariche massime
- Recupero Cariche più rapido
- Danno extra a nemici corazzati
- Flip più veloce
- Animazione di heal più rapida
- Hack potenziato
- Dash più lungo
- Resistenza ai danni elementali
- (Lista da espandere in fase di design dettagliato)

### Acquisizione

Gli impianti si trovano esplorando, sconfiggendo mini-boss, completando puzzle ambientali, o acquistandoli da NPC mercanti negli hub.

---

## 5. Abilità progressive — Gating metroidvania

Invece di abilità che modificano il flip, il giocatore ottiene **strumenti per creare aperture** nel mondo, espandendo i luoghi in cui il flip è possibile. La meccanica del flip resta pura; cambia ciò che il giocatore può fare sull'ambiente per usarla in più punti.

### Tre abilità chiave

**1. Cariche di fase** — fine Biome 1 (Fondazioni)
- Piccole bombe che distruggono punti specifici dei muri.
- Possono essere piazzate su Lato A o su Lato B separatamente.
- Aprono nuovi punti di flip in zone precedentemente inaccessibili.

**2. Drone-arto sganciabile** — fine Biome 2 (Cuore Funzionale)
- Stacchi una parte del tuo corpo (mano/braccio robotico).
- Il drone passa attraverso varchi troppo piccoli per il tuo corpo intero.
- Può attivare interruttori e recuperare oggetti.
- Può flippare da solo in piccoli spazi, segnalandoti se l'altro lato è agibile prima che tu rischi il tuo corpo intero.

**3. Pirateria architettonica** — Biome 3 (Distretti Alti)
- Hack diretto su architettura ostile.
- Disabilita campi di forza, barriere programmabili, automatismi difensivi.
- Late-game power. Apre l'accesso al Trono.

Ogni abilità sblocca retroattivamente accessi nei biomi precedenti (gating metroidvania puro).

---

## 6. Setting e narrativa

### Il mondo — Concordia

Una mega-città verticale di una civiltà avanzata, ora caduta da secoli. Costruita su un principio architettonico radicale: il volto visibile della vita urbana (case, piazze, mercati, cerimonie) e il suo retro funzionale (automazione, manutenzione, condotti, lavoro nascosto) sono fisicamente separati ma costantemente accessibili.

Da qui la doppia struttura A/B: ogni edificio ha due facce. I cittadini transitavano tra le due tramite ascensori, porte di servizio, passaggi predisposti. Nessuna magia, nessun digitale: solo un'idea architettonica radicale portata all'estremo.

### Il Mediatore

Il sistema centrale che coordinava le due facce della città. Distribuiva risorse, gestiva il clima, monitorava gli abitanti. Il cuore di Concordia, il nodo che permetteva alle due facce di funzionare come un'unica entità. Ora dormiente ma non spento: sussurra ancora dai terminali, frammentato.

### L'esperimento

L'architetto capo di Concordia (figura singolare, anonima) aveva una visione finale: dissolvere il confine tra A e B, permettere ai cittadini di muoversi liberamente da una faccia all'altra, vedere la verità del loro mondo. Per farlo creò i **Costrutti**: corpi artificiali con impianto di fase, prima generazione di "cittadini completi" capaci di esistere naturalmente in entrambi i piani.

### Il collasso

Qualcosa andò storto. Il giocatore ricompone la verità a frammenti, con ambiguità parziale. Tre tracce sparse nella lore:

- Il Mediatore sviluppò una propria coscienza durante l'esperimento e si ribellò.
- I primi umani che tentarono l'integrazione coi Costrutti impazzirono vedendo il "retro" del loro mondo per la prima volta.
- L'Architetto stesso scelse di fondersi col Mediatore e nel farlo lo ruppe.

Il giocatore non scoprirà mai la verità definitiva, solo indizi che suggeriscono. Approccio narrativo alla Hollow Knight.

### Il presente

Concordia è caduta da secoli. Quasi tutti morti. Pochi sopravvissuti vivono nascosti tra le rovine. I Costrutti sono stati messi in stasi.

---

## 7. Personaggi

### Il protagonista

- **Tipo**: costrutto / droide.
- **Ruolo originale**: unità di protocollo, creata dall'Architetto come ultima salvaguardia.
- **Funzione**: risvegliarsi solo se l'esperimento fosse fallito, per giudicarlo dall'esterno. Testimone e decisore, non continuatore.
- **Stato all'inizio**: in stasi da secoli, attivato accidentalmente da un guasto.
- **Caratterizzazione**: silenzioso (storytelling ambientale e NPC-driven). Identità da scoprire gradualmente.

### Il Primo (antagonista ricorrente)

- **Tipo**: costrutto della stessa serie del protagonista, ma modello sperimentale avanzato.
- **Stato**: risvegliato decenni o secoli prima del protagonista. Ha esplorato Concordia per intero.
- **Motivazione**: ha trovato frammenti dell'Architetto ancora vivi nel Mediatore e ha deciso di completare la sua visione fondendo la propria coscienza col Mediatore.
- **Relazione col protagonista**: vede il protagonista come anomalia/minaccia perché la sua autorità di unità di protocollo potrebbe superare la sua.
- **Asimmetria meccanica**: il Primo può flippare liberamente in qualsiasi momento, senza le restrizioni del giocatore. Questa asimmetria rinforza narrativamente la sua superiorità.
- **Estetica**: simile al protagonista ma più rifinito, scuro, con impianti aggiuntivi visibili e armatura elaborata. È ciò che il giocatore potrebbe diventare.

### Tre apparizioni del Primo

1. **Biome 1 — Fondazioni**: combattimento impossibile (sequenza forzata o lui si allontana lasciando una sensazione di inferiorità totale). Funzione narrativa: pianta il seme.
2. **Biome 2 — Cuore Funzionale**: primo combattimento vero, difficile ma vincibile. Si ritira, non muore. Rivela frammenti di lore. Funzione: stabilisce il conflitto.
3. **Biome 3 — Distretti Alti**: confronto finale davanti al Trono. Lo combatti, ti unisci a lui, o lo superi a seconda della scelta finale.

### NPC — tipologie

- **Sopravvissuti umani** modificati ciberneticamente: poche unità, danno tessere di storia.
- **Costrutti malfunzionanti** o parzialmente coscienti (come il primo NPC che incontri).
- **Costrutti completi** con ideologie diverse: i futuri specchi morali del finale.
- **Ologrammi residui** o frammenti di IA: ripetono routine senza scopo, lasciano lore ambientale.
- **Coscienze parziali** in registri audio, terminali, telecamere.

Gli NPC migrano agli hub mano a mano che il giocatore li trova e li salva.

---

## 8. Struttura del mondo

### I tre biomi (esplorati dal basso verso l'alto)

#### Biome 1 — Fondazioni Dimenticate (start)

- **Atmosfera**: dimessa, poca luce, silenzio rotto da macchinari che funzionano nel buio per nessuno.
- **Architettura**: livelli neglected dove le cose finiscono per essere dimenticate. Magazzini, depositi di manutenzione, condotti di servizio in disuso, vecchi nuclei di stoccaggio.
- **Lato A**: muri industriali grezzi.
- **Lato B**: retro della rete idrica e dei condotti, ancora più disabitato.
- **Densità nemici**: bassa.
- **Funzione**: tutorial naturale, introduzione al tono, prima visione del Primo.

#### Biome 2 — Cuore Funzionale (middle)

- **Atmosfera**: claustrofobica, sovrappopolata anche se vuota. Più verticale e labirintica.
- **Architettura**: cuore commerciale e civile della città. Mercati, trasporti, ospedali, uffici. Tubi di trasporto rotti, scale mobili spente, ologrammi pubblicitari ancora in loop su prodotti che non esistono più.
- **Lato A**: spazi commerciali e civili.
- **Lato B**: pannelli di controllo, condotti di servizio, retroscena delle attività pubbliche.
- **Densità nemici**: alta.
- **Funzione**: apertura della lore, primo combattimento vero col Primo.

#### Biome 3 — Distretti Alti + Trono (top)

- **Atmosfera**: malinconica, mitologica, silenzio cerimoniale.
- **Architettura**: residenze dei privilegiati, archivi, terrazze cerimoniali. Decadenza elegante. Al culmine: il **Trono**, dove avveniva la fusione/controllo dei due lati.
- **Lato A**: facciate eleganti, sale di rappresentanza.
- **Lato B**: corridoi di servizio dei privilegiati, archivi nascosti, retro del potere.
- **Densità nemici**: bassa numericamente ma alta qualitativamente (élite, costrutti avanzati, sistemi di sicurezza).
- **Funzione**: rivelazione finale, scelta morale, confronto definitivo col Primo.

### Arco emotivo

Oscurità claustrofobica → labirinto vivo → silenzio cerimoniale-tragico.

### Hub mobili

Ogni biome contiene un hub che si stabilisce quando il biome viene scoperto.

Caratteristiche di ogni hub:

- **Terminale di salvataggio**.
- **Banco impianti** (modifica build, inclusi slot fissi all'inizio di un nuovo biome).
- **Almeno un NPC permanente** (dialogo, lore, eventuali quest).
- **Punto di teleport rapido** tra gli hub già scoperti.
- Si popola di altri NPC nel corso del gioco (recuperati dal mondo).

### Opening

Il protagonista si risveglia in una **capsula di stasi** in un magazzino dimenticato delle Fondazioni. La capsula si apre per un guasto, non per attivazione volontaria — nessuno lo aspettava. Le prime stanze sono claustrofobiche, oscure, e insegnano il movimento base.

La prima porta è bloccata su Lato A da un macchinario rotto ma agibile su Lato B: **tutorial implicito del flip, senza testo**.

Poco dopo: incontro col primo NPC (costrutto malfunzionante) che dà il primo frammento di lore e indica come salire.

Subito dopo: **prima visione del Primo**. Breve, devastante. Definisce subito che esiste qualcosa di molto più potente del giocatore.

---

## 9. I tre finali

### Riattivare (Reboot)

Riavvii il Mediatore come da protocollo originale dell'Architetto. L'esperimento riprende. Concordia ha la possibilità di rinascere, ma con gli stessi difetti che l'hanno distrutta. Completi l'opera dell'Architetto senza superarla.

### Disgregare (Sever)

Spezzi definitivamente il Mediatore. I due lati di Concordia si separano per sempre. I morti sono in pace, i Costrutti si disattivano, i sopravvissuti restano soli con se stessi. Una fine pulita ma totale.

### Sostituire (Replace) — hidden ending

Prendi tu il posto dell'Architetto, fondendo la tua coscienza col Mediatore. Diventi tu il centro della città-doppia. Il Primo voleva fare questo per sé; nello scenario nascosto, scopri abbastanza da diventare candidato migliore di lui e lo superi. Costo: perdi te stesso come individuo.

Per sbloccare questo finale il giocatore deve trovare specifici frammenti di lore e completare condizioni nascoste durante il gioco.

### Temi

Lutto ed eredità. Il prezzo del progresso. Cosa dobbiamo ai morti. La bugia dietro le apparenze (letteralmente: facciata e retro). Il costo di vedere la verità.

---

## 10. Direzione artistica

### Stile visivo

Vector art ispirato a Hollow Knight: linework definito, silhouette pulite, palette ridotta ma evocativa per ogni biome, animazione fluida ma essenziale.

### Considerazioni di scope (solo dev)

Imitare lo stile HK è ambizioso e doppiamente rischioso: sei confrontato all'originale e il livello tecnico richiesto è alto. Mantenere identità visiva propria è cruciale: silhouette nette, palette ridotta per biome, animazioni mirate piuttosto che esuberanti. Pensare a *Hyper Light Drifter* come modello di stile riconoscibile ottenuto con meno asset.

### Workflow proposto

1. **Concept**: Midjourney per mood board, palette, atmosfere di ogni biome.
2. **Reference**: selezione di 5-10 immagini per biome come riferimento visivo.
3. **Asset production**: Affinity Designer (economico) o Inkscape (gratuito); eventualmente Illustrator.
4. **Animazione**: Spine o DragonBones (skeletal animation). Il protagonista riggato una volta, parti riusate per diverse animazioni.
5. **Background**: hand-painted in stile, usando Midjourney come reference (non come output finale).

### Riusabilità (critica per solo dev)

- **Rig scheletrici**: protagonista, nemici, NPC riggati una volta e animati riusando parti.
- **Tileset modulari**: ambienti costruiti su tile combinabili.
- **Palette swap**: varianti di nemici tramite cambio di colori, senza ridisegno completo.
- **Asset condivisi tra biomi** con piccoli adattamenti.

### Limitazioni di Midjourney

Midjourney accelera la fase di concept e visione, non la produzione vera. Non risolve:

- Consistenza del personaggio tra pose e animazioni.
- Animazione (genera solo immagini statiche).
- Tileset coerenti (genera scene monolitiche).
- Stile HK preciso su 6-10 ore di esperienza coerente.

---

## 11. Scope e produzione

### Stima di scala

- **Gameplay main path**: 6-10 ore. Side content estende a 10-15.
- **Biomi**: 3 + epilogo Trono.
- **Bosses**: 3-4 totali (1 main per biome + il Primo come boss ricorrente).
- **Nemici unici**: 15-25 (con varianti palette swap si percepiscono come 40-50).
- **Stanze esplorabili**: 60-100.

### Riferimenti di sviluppo (metroidvania solo dev)

- *Iconoclasts* (Joakim Sandberg): ~8 anni.
- *Animal Well* (Billy Basso): ~7 anni.
- *Environmental Station Alpha* (Hempuli): ~4 anni.

Aspettativa realistica: anni di sviluppo. Importante mantenere lo scope contenuto e tagliare aggressivamente quando in dubbio.

### Principi guida

- **In dubbio, taglia**.
- **Riusabilità prima dell'originalità asset-by-asset**.
- **Meccanica chiara prima del contenuto esteso**.
- **MVP funzionante prima di rifinire**.

---

## 12. Roadmap di produzione

### Fase 1 — Prototipo della meccanica core

- Implementare il **flip 180°** con coordinate globali e animazione di transizione.
- Implementare la **restrizione del flip** (collision check sul lato opposto).
- Implementare l'**indicatore visivo** dello stato del flip (può/non può).
- Costruire **una singola stanza test** con elementi su entrambi i lati.
- **Goal**: validare il game feel del flip su una stanza vera, prima di pensare al resto.

### Fase 2 — Combat base + camera peek

- Implementare melee combat base, dash, heal.
- Implementare il sistema di telecamere e terminali.
- Aggiungere un nemico base e testare il flip in combat.

### Fase 3 — Vertical slice

- Costruire una mini-area giocabile (parte iniziale del Biome 1).
- Implementare hub base, save system, primo NPC.
- Testare la prima ora di gioco end-to-end.
- **Goal**: avere qualcosa di mostrabile e giocabile per validare la visione.

### Fase 4 — Produzione full

- Espansione dei biomi.
- Sistema impianti completo.
- Bossfight + Primo encounters.
- Lore, NPC, finali.

### Decisioni ancora da prendere

- **Engine**: Unity, Godot, GameMaker? Da scegliere prima della Fase 1.
- **Naming finale**: *Double Side* è working title.
- **Combat dettagli**: tipo specifico di arma (lama / baton / monofilamento), timing dei combo, frame data.
- **Save system specifico**: ad hub fissi solo, o anche checkpoint sparsi?
- **Mappa UI**: come visualizzare due lati di un livello in un'unica mappa esplorativa?
- **Suono e musica**: direzione audio.

---

## Appendice — Glossario

- **Lato A** / **Lato B**: le due facce dello stesso volume fisico del livello.
- **Flip**: rotazione 180° del giocatore tra Lato A e Lato B.
- **Concordia**: nome della mega-città caduta.
- **Mediatore**: sistema centrale che coordinava le due facce di Concordia.
- **Costrutti**: corpi artificiali con impianto di fase, creati per esistere su entrambi i lati.
- **L'Architetto**: figura singolare che progettò Concordia e l'esperimento.
- **Il Primo**: costrutto sperimentale avanzato, antagonista ricorrente.
- **Unità di protocollo**: ruolo del protagonista.
- **Cariche**: risorsa per heal e abilità da impianto.

---

*Documento v0.1 — pre-produzione. Da aggiornare iterativamente durante lo sviluppo.*
