# Hermes.Domain

## Overview

`Hermes.Domain` definisce il **modello concettuale del sistema Hermes**.

Il Domain rappresenta i concetti fondamentali attraverso cui Hermes gestisce:

- soggetti che utilizzano il sistema;
- operazioni;
- sistemi e destinazioni;
- percorsi configurati;
- richieste concrete;
- esecuzioni;
- step di elaborazione.

Il dominio è organizzato intorno a cinque concetti principali:

| Concept | Description |
|---|---|
| **Client** | Chi utilizza Hermes |
| **OperationType** | Quale operazione viene richiesta o gestita |
| **Endpoint** | Verso quale sistema può essere indirizzata l'operazione |
| **Route** | La combinazione `Client → OperationType → Endpoint` |
| **Execution** | Come una `Operation` viene elaborata attraverso uno o più step |

A questi concetti si aggiungono le entità che rappresentano le relazioni e la struttura dell'esecuzione:

- `ClientOperation`
- `EndpointOperation`
- `Operation`
- `ExecutionStep`
- `ExecutionStepRoute`
- `ExecutionType`
- `ExecutionStepType`

## Client

`Client` rappresenta il **sistema, applicazione o soggetto che utilizza Hermes** per effettuare un'operazione.

Il `Client` non definisce direttamente quali operazioni può eseguire.

Le operazioni disponibili vengono configurate attraverso `ClientOperation`.

### Relationship

```text
Client N:N OperationType
       │
       └── ClientOperation

a relazione permette di stabilire quali OperationType sono disponibili per ciascun Client.

Responsibility

Il Client identifica quindi chi utilizza Hermes.

Esempi concettuali:

WebApplication
MobileApplication
InternalService
ExternalService
ScheduledProcess

---

### BLOCCO 3 — OperationType

```markdown
## OperationType

`OperationType` rappresenta il **tipo di operazione che Hermes conosce e gestisce**.

È un'entità configurabile e registrabile.

Non viene modellata come `enum`, perché il catalogo delle operazioni può evolvere nel tempo.

### Examples

```text
CREATE_USER
UPDATE_USER
DELETE_USER
SYNCHRONIZE_DATA
IMPORT_DATA
EXPORT_DATA

Un OperationType può essere:

utilizzato da più Client;
gestito da più Endpoint.
Relationships
Client N:N OperationType

OperationType N:N Endpoint

Questa struttura permette di mantenere il catalogo delle operazioni indipendente dai Client e dagli Endpoint.


---

### BLOCCO 4 — ClientOperation

```markdown
## ClientOperation

`ClientOperation` rappresenta la **relazione tra un Client e un OperationType**.

Indica quali operazioni un determinato `Client` è autorizzato o abilitato a utilizzare.

### Relationship

```text
┌──────────┐
│  Client  │
└────┬─────┘
     │
     │ N:N
     ▼
┌─────────────────┐
│ ClientOperation │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  OperationType  │
└─────────────────┘

La relazione è esplicita per poter essere configurata e abilitata o disabilitata indipendentemente.

Questa struttura permette inoltre di aggiungere in futuro informazioni specifiche sulla relazione tra Client e OperationType.


---

### BLOCCO 5 — Endpoint

```markdown
## Endpoint

`Endpoint` rappresenta un **sistema o una destinazione con cui Hermes può comunicare**.

L'Endpoint identifica la destinazione a livello di dominio, senza definire nel Domain la tecnologia utilizzata per raggiungerla.

### Examples

- CRM
- ERP
- Moodle
- DataPlatform
- ExternalSystem

Un `Endpoint` può gestire più `OperationType`.

### Technology Independence

Il Domain non deve conoscere la tecnologia utilizzata per raggiungere l'Endpoint.

Ad esempio:

```text
HTTP
REST
SOAP
Message Queue
Database
gRPC
SDK

Questi dettagli appartengono ai layer infrastrutturali e di integrazione.

Il Domain conosce quindi la destinazione, ma non come tecnicamente raggiungerla.


---

### BLOCCO 6 — EndpointOperation

```markdown
## EndpointOperation

`EndpointOperation` rappresenta la **relazione tra Endpoint e OperationType**.

Indica quali operazioni un determinato `Endpoint` è in grado di gestire.

### Relationship

```text
┌──────────────┐
│   Endpoint   │
└──────┬───────┘
       │
       │ N:N
       ▼
┌──────────────────┐
│ EndpointOperation│
└────────┬─────────┘
         │
         ▼
┌─────────────────┐
│  OperationType  │
└─────────────────┘

In questo modo Hermes può distinguere chiaramente:

Client
  │
  └── quali operazioni può utilizzare
          │
          ▼
     ClientOperation


Endpoint
  │
  └── quali operazioni può gestire
          │
          ▼
     EndpointOperation

---

### BLOCCO 7 — Route

```markdown
## Route

`Route` rappresenta una **specifica combinazione configurata**:

```text
Client → OperationType → Endpoint

Una Route definisce quindi un percorso disponibile all'interno di Hermes.

Conceptual Model
┌──────────┐
│  Client  │
└────┬─────┘
     │
     ▼
┌─────────────────┐
│  OperationType  │
└────┬────────────┘
     │
     ▼
┌──────────┐
│ Endpoint │
└──────────┘
Example
Client A → CREATE_USER → CRM

Una Route è una configurazione riutilizzabile.

Non rappresenta l'esecuzione concreta di un'operazione.

La distinzione fondamentale è:

Route
    = configurazione del percorso

Operation
    = richiesta concreta

Execution
    = elaborazione concreta della richiesta

---

### BLOCCO 8 — Operation

```markdown
## Operation

`Operation` rappresenta una **richiesta concreta ricevuta da Hermes**.

È l'istanza dell'operazione che deve essere gestita dal sistema.

### Conceptual Model

```text
Operation
├── Id
├── ExecutionId
├── CorrelationId
├── ExternalId
└── Status

L'Operation rappresenta quindi la richiesta concreta e il relativo processo di esecuzione.

Separation from Configuration

L'Operation non contiene direttamente:

ClientId
EndpointId
OperationTypeId
RouteId

Il percorso e la configurazione appartengono alle Route.

L'Operation rappresenta invece la richiesta concreta.

Configuration
      │
      └── Route


Runtime
      │
      └── Operation

Questa separazione permette di mantenere distinta la configurazione del sistema dall'esecuzione runtime.


---

### BLOCCO 9 — Execution

```markdown
## Execution

`Execution` rappresenta l'**esecuzione complessiva di una Operation**.

La relazione è:

```text
Operation 1:1 Execution
Relationship
┌─────────────┐
│  Operation  │
└──────┬──────┘
       │
       │ 1:1
       ▼
┌─────────────┐
│  Execution  │
└─────────────┘

Ogni Operation possiede una Execution associata.

La Execution rappresenta il ciclo di vita dell'elaborazione complessiva della richiesta.


---

### BLOCCO 10 — ExecutionStep

```markdown
## ExecutionStep

`ExecutionStep` rappresenta una **singola fase dell'esecuzione**.

Una `Execution` può contenere un numero arbitrario di Step.

### Relationship

```text
Execution 1:N ExecutionStep
Example
Execution
│
├── Step 1
├── Step 2
├── Step 3
└── Step N

Ogni Step possiede:

una sequenza;
un proprio stato di esecuzione;
i tipi di elaborazione associati;
le Route utilizzate nella specifica fase.

Lo Step è il punto in cui vengono definite le attività e i percorsi necessari per quella specifica fase dell'esecuzione.


---

### BLOCCO 11 — ExecutionStepRoute

```markdown
## ExecutionStepRoute

`ExecutionStepRoute` rappresenta la relazione tra `ExecutionStep` e `Route`.

### Relationship

```text
ExecutionStep N:N Route

Uno Step può utilizzare più Route e la stessa Route può essere riutilizzata da più Step.

┌────────────────┐
│ ExecutionStep  │
└───────┬────────┘
        │
        │ N:N
        ▼
┌────────────────────┐
│ ExecutionStepRoute │
└─────────┬──────────┘
          │
          ▼
       ┌───────┐
       │ Route │
       └───────┘

Questo permette a uno stesso Step di coinvolgere più sistemi o più percorsi.

Example
Step 1
│
├── Route A
│      Client A → Operation X → System B
│
└── Route B
       Client A → Operation X → System C

---

### BLOCCO 12 — ExecutionType

```markdown
## ExecutionType

`ExecutionType` rappresenta il **tipo o la categoria di elaborazione associata a uno Step**.

È un'entità configurabile e non un `enum`.

Il catalogo dei tipi di elaborazione può quindi evolvere nel tempo.

### Examples

```text
Processing
Integration
BackgroundProcessing
DataStorage
Validation
Transformation

Un ExecutionType può essere utilizzato da più ExecutionStep.


---

### BLOCCO 13 — ExecutionStepType

```markdown
## ExecutionStepType

`ExecutionStepType` rappresenta la relazione tra `ExecutionStep` e `ExecutionType`.

### Relationship

```text
ExecutionStep N:N ExecutionType
┌────────────────┐
│ ExecutionStep  │
└───────┬────────┘
        │
        │ N:N
        ▼
┌────────────────────┐
│ ExecutionStepType  │
└─────────┬──────────┘
          │
          ▼
┌─────────────────┐
│  ExecutionType  │
└─────────────────┘

Uno Step può quindi avere più tipi di elaborazione.

Lo stesso ExecutionType può essere riutilizzato da più Step.

Example
Step 1
│
├── Processing
└── Integration

Step 2
│
└── DataStorage

Step 3
│
└── Processing

---

### BLOCCO 14 — Execution Flow

```markdown
# Execution Flow

Una singola `Operation` può produrre un'esecuzione composta da più Step.

```text
Operation
    │
    │ 1:1
    ▼
Execution
    │
    │ 1:N
    ▼
ExecutionStep
    │
    ├── ExecutionTypes
    │
    └── Routes
Complete Example
Operation
│
└── Execution
    │
    ├── Step 1
    │   │
    │   ├── ExecutionTypes
    │   │   ├── Processing
    │   │   └── Integration
    │   │
    │   └── Routes
    │       ├── Client A → Operation X → System B
    │       └── Client A → Operation X → System C
    │
    ├── Step 2
    │   │
    │   ├── ExecutionTypes
    │   │   └── DataStorage
    │   │
    │   └── Routes
    │       └── System C → Operation Y → DataStore
    │
    └── Step 3
        │
        ├── ExecutionTypes
        │   └── Processing
        │
        └── Routes
            └── DataStore → Operation Z → System D

In questo modello:

la Route descrive un percorso configurato;
l'ExecutionStep determina quali percorsi vengono utilizzati in quella fase;
l'ExecutionType descrive il tipo di elaborazione dello Step;
la Execution rappresenta l'intero processo;
l'Operation rappresenta la richiesta concreta.

---

### BLOCCO 15 — Cardinalità

```markdown
# Relationships & Cardinalities

| Relationship | Cardinality | Relationship Entity |
|---|---:|---|
| `Client` → `OperationType` | N:N | `ClientOperation` |
| `Endpoint` → `OperationType` | N:N | `EndpointOperation` |
| `Operation` → `Execution` | 1:1 | — |
| `Execution` → `ExecutionStep` | 1:N | — |
| `ExecutionStep` → `Route` | N:N | `ExecutionStepRoute` |
| `ExecutionStep` → `ExecutionType` | N:N | `ExecutionStepType` |

## N:N Relationships

Le relazioni N:N sono rappresentate da entità dedicate:

```text
ClientOperation
EndpointOperation
ExecutionStepRoute
ExecutionStepType

Questo permette di mantenere le relazioni esplicite ed estendibili.


---

### BLOCCO 16 — Configuration vs Runtime

```markdown
# Configuration vs Runtime

Una distinzione fondamentale del Domain Hermes è quella tra **Configuration** e **Runtime**.

## Configuration

La configurazione rappresenta ciò che Hermes è configurato per poter fare.

```text
Client
   │
   │ N:N
   ▼
ClientOperation
   │
   ▼
OperationType
   ▲
   │
   │ N:N
   │
EndpointOperation
   ▲
   │
   │
Endpoint

Client + OperationType + Endpoint
                │
                ▼
              Route

La configurazione è riutilizzabile.

Runtime

Il runtime rappresenta ciò che Hermes sta effettivamente elaborando.

Operation
    │
    │ 1:1
    ▼
Execution
    │
    │ 1:N
    ▼
ExecutionStep
    │
    ├── ExecutionType
    │
    └── ExecutionStepRoute
              │
              ▼
            Route

La runtime execution utilizza quindi la configurazione esistente per realizzare il processo concreto.


---

### BLOCCO 17 — Domain Responsibilities

```markdown
# Domain Responsibilities

Il Domain ha la responsabilità di rappresentare:

- quali soggetti interagiscono con Hermes;
- quali operazioni Hermes conosce;
- quali sistemi possono essere raggiunti;
- quali combinazioni `Client / OperationType / Endpoint` sono configurate;
- quali richieste vengono ricevute;
- come una richiesta viene eseguita;
- quali Step compongono un'esecuzione;
- quali tipi di elaborazione appartengono agli Step.

Il Domain **non deve invece conoscere il HOW tecnologico**.

Ad esempio, il Domain non deve sapere se un Endpoint viene raggiunto tramite:

```text
HTTP
REST
SOAP
RabbitMQ
Kafka
Database
gRPC
SDK

Questi dettagli appartengono ai layer esterni, in particolare:

Application
Infrastructure
Integration

Il Domain conosce quindi:

WHAT Hermes deve rappresentare

ma non:

HOW Hermes deve tecnicamente implementarlo.


---

### BLOCCO 18 — Domain Principles

```markdown
# Domain Principles

## 1. Configuration is not Runtime

La configurazione del sistema è separata dall'esecuzione concreta.

```text
Route
   ≠
Operation

La Route descrive come Hermes può essere configurato.

L'Operation descrive una richiesta concreta.

2. OperationType is Configurable

OperationType è un'entità e non un enum.

Questo permette di evolvere il catalogo delle operazioni senza legare il Domain a un insieme statico di valori.

3. ExecutionType is Configurable

Anche ExecutionType è un'entità e non un enum.

Questo permette di aggiungere nuove categorie di elaborazione senza modificare necessariamente il modello applicativo.

4. Explicit N:N Relationships

Le relazioni N:N vengono rappresentate esplicitamente tramite entità dedicate:

ClientOperation
EndpointOperation
ExecutionStepRoute
ExecutionStepType

Questo mantiene il modello estendibile e permette di aggiungere informazioni alle relazioni.

5. Technology Independence

Il Domain non conosce le tecnologie utilizzate per comunicare con gli Endpoint.

Domain
   │
   │ knows
   ▼
Endpoint

Domain
   │
   │ does NOT know
   ▼
HTTP / SOAP / Kafka / RabbitMQ / DB / gRPC

La tecnologia viene definita nei layer esterni.


---

### BLOCCO 19 — Summary

```markdown
# Summary

| Concept | Responsibility |
|---|---|
| **Client** | Chi utilizza Hermes |
| **OperationType** | Quale operazione Hermes conosce e gestisce |
| **Endpoint** | Con quale sistema o destinazione Hermes può interagire |
| **ClientOperation** | Quali OperationType un Client può utilizzare |
| **EndpointOperation** | Quali OperationType un Endpoint può gestire |
| **Route** | Percorso configurato `Client → OperationType → Endpoint` |
| **Operation** | Richiesta concreta ricevuta da Hermes |
| **Execution** | Esecuzione complessiva della richiesta |
| **ExecutionStep** | Singola fase dell'esecuzione |
| **ExecutionType** | Tipo di elaborazione associato a uno Step |
| **ExecutionStepRoute** | Route utilizzate da uno Step |
| **ExecutionStepType** | Tipi di elaborazione associati a uno Step |
BLOCCO 20 — Final Definition
# Final Definition

> **Hermes.Domain definisce il modello concettuale attraverso cui Hermes rappresenta Client, OperationType, Endpoint e Route come configurazione del sistema, e Operation, Execution e ExecutionStep come modello runtime dell'elaborazione.**

In sintesi:

```text
Client = chi utilizza Hermes

OperationType = cosa viene richiesto

Endpoint = con quale sistema/destinazione interagire

ClientOperation = cosa un Client può utilizzare

EndpointOperation = cosa un Endpoint può gestire

Route = percorso configurato  Client → OperationType → Endpoint

Operation = richiesta concreta

Execution = esecuzione della richiesta

ExecutionStep = singola fase dell'esecuzione

ExecutionType = tipo di elaborazione dello Step

ExecutionStepRoute = Route utilizzate dallo Step

ExecutionStepType = tipi associati allo Step
