Hermes.Domain

Il Domain di Hermes definisce il modello concettuale del sistema: soggetti, operazioni, destinazioni, percorsi ed esecuzioni.

Il dominio è organizzato intorno a cinque concetti principali:

Client — chi utilizza Hermes.
OperationType — quale operazione viene richiesta o gestita.
Endpoint — verso quale sistema può essere indirizzata l'operazione.
Route — la combinazione Client → OperationType → Endpoint.
Execution — come una Operation viene elaborata attraverso uno o più step.


Client

Il Client rappresenta il sistema, applicazione o soggetto che utilizza Hermes per effettuare un'operazione.

Un Client non definisce direttamente quali operazioni può eseguire: queste vengono configurate attraverso ClientOperation.

Client N:N OperationType
        │
        └── ClientOperation

La relazione permette di stabilire quali OperationType sono disponibili per ciascun Client.

OperationType

OperationType rappresenta il tipo di operazione che Hermes conosce e gestisce.

È un'entità configurabile e registrabile, non un enum, perché il catalogo delle operazioni può evolvere nel tempo.

Esempi:

CREATE_USER
UPDATE_USER
DELETE_USER
SYNCHRONIZE_DATA
IMPORT_DATA
EXPORT_DATA

Un OperationType può essere utilizzato da più Client e gestito da più Endpoint.

Client N:N OperationType N:N Endpoint
ClientOperation

ClientOperation rappresenta la relazione tra Client e OperationType.

Indica quali operazioni un determinato Client è autorizzato o abilitato a utilizzare.

Client
   │
   │ N:N
   ▼
ClientOperation
   │
   ▼
OperationType

La relazione è esplicita per poter essere configurata e abilitata/disabilitata indipendentemente.

Endpoint

Endpoint rappresenta un sistema o destinazione con cui Hermes può comunicare.

L'Endpoint identifica la destinazione a livello di dominio, senza definire nel Domain la tecnologia utilizzata per raggiungerla.

Esempi concettuali:

CRM
ERP
Moodle
DataPlatform
ExternalSystem

Un Endpoint può gestire più OperationType.

EndpointOperation

EndpointOperation rappresenta la relazione tra Endpoint e OperationType.

Indica quali operazioni un determinato Endpoint è in grado di gestire.

Endpoint
   │
   │ N:N
   ▼
EndpointOperation
   │
   ▼
OperationType

In questo modo Hermes può distinguere:

Client → quali operazioni può utilizzare

Endpoint → quali operazioni può gestire
Route

Route rappresenta una specifica combinazione configurata:

Client → OperationType → Endpoint

Una Route definisce quindi un percorso disponibile all'interno di Hermes.

┌──────────┐      ┌───────────────┐      ┌──────────┐
│  Client  │ ───► │ OperationType │ ───► │ Endpoint │
└──────────┘      └───────────────┘      └──────────┘
                         │
                         │
                       Route

Una Route è una configurazione riutilizzabile e non rappresenta l'esecuzione concreta di un'operazione.

Operation

Operation rappresenta una richiesta concreta ricevuta da Hermes.

È l'istanza dell'operazione che deve essere gestita dal sistema.

Operation
├── Id
├── ExecutionId
├── CorrelationId
├── ExternalId
└── Status

L'Operation non contiene direttamente ClientId, EndpointId, OperationTypeId o RouteId.

Il percorso e la configurazione appartengono alle Route; l'Operation rappresenta invece la richiesta concreta e il relativo processo di esecuzione.

Execution

Execution rappresenta l'esecuzione complessiva di una Operation.

La relazione è:

Operation 1:1 Execution

Ogni Operation possiede una Execution associata.

La Execution rappresenta il ciclo di vita dell'elaborazione complessiva.

Operation
    │
    │ 1:1
    ▼
Execution
ExecutionStep

ExecutionStep rappresenta una singola fase dell'esecuzione.

Una Execution può contenere un numero arbitrario di step.

Execution 1:N ExecutionStep

Esempio:

Execution
│
├── Step 1
├── Step 2
├── Step 3
└── Step N

Ogni step possiede una sequenza e un proprio stato di esecuzione.

Lo Step è il punto in cui vengono definite le attività e i percorsi necessari per quella specifica fase.

ExecutionStepRoute

ExecutionStepRoute rappresenta la relazione N tra ExecutionStep e Route.

ExecutionStep N:N Route

Uno Step può utilizzare più Route e la stessa Route può essere riutilizzata da più Step.

ExecutionStep
      │
      │ N:N
      ▼
ExecutionStepRoute
      │
      ▼
    Route

Questo permette a uno stesso Step di coinvolgere più sistemi o più percorsi.

ExecutionType

ExecutionType rappresenta il tipo o la categoria di elaborazione associata a uno Step.

È un'entità configurabile e non un enum.

Esempi:

Processing
Integration
BackgroundProcessing
DataStorage
Validation
Transformation

Un ExecutionType può essere utilizzato da più ExecutionStep.

ExecutionStepType

ExecutionStepType rappresenta la relazione N tra ExecutionStep e ExecutionType.

ExecutionStep N:N ExecutionType

Uno Step può quindi avere più tipi di elaborazione e lo stesso tipo può essere riutilizzato da più Step.

ExecutionStep
      │
      │ N:N
      ▼
ExecutionStepType
      │
      ▼
ExecutionType
Execution Flow

Una singola Operation può quindi produrre un'esecuzione composta da più step.

Operation
    │
    │ 1:1
    ▼
Execution
    │
    │ 1:N
    ▼
┌──────────────────────────────────────────────┐
│                ExecutionSteps                │
│                                              │
│  Step 1                                      │
│  ├── ExecutionTypes                          │
│  │   ├── Processing                          │
│  │   └── Integration                         │
│  │                                           │
│  └── Routes                                  │
│      ├── Client A → Operation X → System B   │
│      └── Client A → Operation X → System C   │
│                                              │
│  Step 2                                      │
│  ├── ExecutionTypes                          │
│  │   └── DataStorage                         │
│  │                                           │
│  └── Routes                                  │
│      └── System C → Operation Y → DataStore  │
│                                              │
│  Step 3                                      │
│  ├── ExecutionTypes                          │
│  │   └── Processing                          │
│  │                                           │
│  └── Routes                                  │
│      └── DataStore → Operation Z → System D  │
└──────────────────────────────────────────────┘

Cardinalità
Client           N:N OperationType
Endpoint         N:N OperationType

Operation        1:1 Execution

Execution        1:N ExecutionStep

ExecutionStep    N:N Route
ExecutionStep    N:N ExecutionType

Le relazioni N sono rappresentate da entità dedicate:

ClientOperation
EndpointOperation
ExecutionStepRoute
ExecutionStepType


====================================================================================

In sintesi:

Client = chi utilizza Hermes.
OperationType = cosa viene richiesto.
Endpoint = con quale sistema/destinazione interagire.
ClientOperation = cosa un Client può utilizzare.
EndpointOperation = cosa un Endpoint può gestire.
Route = percorso configurato Client → OperationType → Endpoint.
Operation = richiesta concreta.
Execution = esecuzione della richiesta.
ExecutionStep = singola fase dell'esecuzione.
ExecutionType = tipo di elaborazione dello Step.
ExecutionStepRoute = Route utilizzate dallo Step.
ExecutionStepType = tipi associati allo Step.