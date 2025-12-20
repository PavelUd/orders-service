## `OrdersService`

Orders Service — это учебно-прикладной проект, реализующий микросервисную систему управления заказами с использованием событийного взаимодействия и паттерна Saga.

## Структура

```
Contracts/
 └─ Commands/           – общие команды
 └─ Events/             – общие события

OrderService/
 ├─ API/                   – HTTP API
 ├─ Application/           – бизнес-логика, хендлеры
 ├─ Domain/                – доменные модели
 ├─ Infrastructure/        – доступ к данным (EF Core)
 └─ Migrations/            – миграции БД

InventoryService/
 ├─ API/
 ├─ Application/
 ├─ Domain/
 ├─ Infrastructure/
 └─ Migrations/

PaymentService/
 ├─ API/
 ├─ Application/
 ├─ Domain/
 ├─ Infrastructure/
 └─ Migrations/

SagaCoordinator/
 ├─ OrderSaga.cs           – логика саги
 └─ OrderSagaData.cs       – состояние саги
```
