namespace Client
{
    public class TCP_Client_GetObj
    {
        private object _Object;
        private readonly object _FirstSelector;
        ///  <summary> 
        ///0 - код проверки(OK)
        ///1 - авторизация
        ///   0 - возвращает имена из кэша
        ///   1 - возвращает токен
        ///2 - Работа с позициями
        ///   0 - возвращает названия позиции из кэша(поиск)
        ///   1 - возвращает позицию как класс с картинкой
        ///   2 - обрабатываем прайс с почты и возвращаем Compare_Item_Result
        ///3 - CRM
        ///   0 - поиск партнера по имени
        ///   1 - возвращаем ивенты по партнеру
        ///   2 - добавляем партнера
        ///   3 - удаляем партнера
        ///   4 - добавляем ивент
        ///   5 - удаляем ивент
        ///4 - почта
        ///5 - сообщение
        ///6 - работа с сайтом
        ///  </summary>
        public TCP_Client_GetObj(object FirstSelector) => _FirstSelector = FirstSelector;

        public object Get(object SecondSelector, object Object = null, object ThirdSelector = null)
        {
            _Object = Object;
            object Obj = null;
            try
            {
                Obj = new TCP_Client(new object[] { _FirstSelector, SecondSelector, ThirdSelector }, Token: Data.Token, SendObj: _Object).Data.Obj;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

            return Obj;
        }

    }
}
