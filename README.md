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
 ├─ Controllers/
 ├─ Application/
 ├─ Domain/
 ├─ Infrastructure/
 └─ Migrations/

SagaCoordinator/
 ├─ OrderSaga.cs           – логика саги
 └─ OrderSagaData.cs       – состояние саги
```

## Environment

RabbitUser=имя учётки в rabbitMq

RabbitPwd=пароль от учётки в rabbitMq

RabbitHost=rabbitmq

RabbitPort=порт RabbitMq

RabbitVhost= vhost в rabbitMq

DbHost=postgres

DbPort=порт бд

SagaDbName=имя бд для saga coordinator-а

PaymentDbName=название бд для сервиса оплаты

InventoryDbName=название бд со всеми товарами

OrderDbName=название бд с заказами

DbUser=имя пользователя в бд

DbPwd=пароль от бд
