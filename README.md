 Настройка приложения 
 1. Необходимо подключить имеющийся файл Items.mdf с базой данных к серверу;
 2. В файле App.config в секции ConnectionStrings в строке connectionString изменить значение (localdb)\MSSQLLocalDB на имя базы данных. 
    В приложении используется windows authentification
 3. Изначально приложение настроено для роботы с табицей items базы данных Items.mdf.
    Для роботы с другими таблицами, слудуещее:  
    В файле App.config в строке add key="table_name" value="items" изменить значение value на название таблицы.
    В файле App.config в строке  add key="columns_groupby" value="organization,city,manager,date" изменить значение value на имеена столбцов, 
    по которым возможна групировка
    В файле App.config в строке  add key="columns_sum_by" value="count,sum" изменить значение value на имеена столбцов, 
    по которым возможно суммирование. 