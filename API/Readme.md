# Config

Для локальной разработки нужны настройки. Приложение читает их из файла `Config`. Создать здесь же, где и Readme.md.
Посмотреть переменные, которые нужно просетить можно в `ConfigReader.cs`.
Обязательно разделитель вида ` = `

``` C#
DATABASE_CONNECTION_STRING = Server=localhost;Port={порт(обычно 5432)};Database=crimeanBilling;User Id={Ваш юзер};Password={Пароль юзера}
```