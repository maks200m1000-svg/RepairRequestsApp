# Программа для сбора заявок на ремонт помещения

Приложение на C# + Windows Forms для сбора заявок на ремонт помещения

Структура проекта
RepairRequestsApp/
Models/
RepairRequest.cs - Заявки на ремонт
Services/
DatabaseService.cs - Сервис работы с БД
Forms/
MainForm.cs - Главная форма приложения
MainForm.Designer.cs - Дизайнер главной формы
AddEditRequestForm.cs - Форма добавления/редактирования
AddEditRequestForm.Designer.cs
Program.cs - Точка входа приложения

## Технологии

* **.NET 10.0** - Windows Forms приложение
* **SQLite** - легковесная встроенная база данных
* **Dapper** - микро-ORM для работы с БД
* **System.Data.SQLite.Core** - ADO.NET провайдер для SQLite

## База данных

База данных **RepairRequests.db**

## Автор Гриценко Максим

Создано для управления заявками на ремонт помещений

