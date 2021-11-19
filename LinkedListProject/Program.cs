using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedListProject
{
    /// <summary>
    ///     Реализация списка
    /// </summary>
    /// <typeparam name="T">Тип элементов списка</typeparam>
    public class SinglyLinkedList<T>
    {
        /// <summary>
        ///     Указатель на начало списка
        /// </summary>
        private SinglyLinkedListNode Head { get; set; }

        /// <summary>
        ///     Возвращает элемент по индексу в списке.
        /// </summary>
        /// <param name="key">Индекс возвращаемого элемента</param>
        /// <returns>Элемент по индексу</returns>
        public T this[int key] => GetElementByIndex(key);

        /// <summary>
        ///     Проверка на пустоту
        /// </summary>
        public bool IsEmpty => GetLength() == 0;

        /// <summary>
        ///     Добавляет новую ноду в начало списка,
        ///     Временная сложность: O(1).
        /// </summary>
        /// <param name="data">Содержание новой ноды.</param>
        /// <returns>Добавленная новая нода.</returns>
        public SinglyLinkedListNode AddFirst(T data)
        {
            // Объявляем новую ноду с данными и ссылкой на голову
            var newListElement = new SinglyLinkedListNode(data)
            {
                Next = Head
            };
            Head = newListElement;
            // Возвращаем новосозданную ноду
            return newListElement;
        }

        /// <summary>
        ///     Добавить новую ноду в конец списка,
        ///     временная сложность: O(n),
        ///     где n - количество нод в списке.
        /// </summary>
        /// <param name="data">Содержание новой ноды.</param>
        /// <returns>Добавленная новая нода.</returns>
        public SinglyLinkedListNode AddLast(T data)
        {
            // Создаём новый элемент списка
            var newListElement = new SinglyLinkedListNode(data);

            // Если голова пуста, то добавленный элемент - первый
            if (Head is null)
            {
                Head = newListElement;
                return newListElement;
            }

            // Временный ListElement, чтобы избежать перезаписи оригинала
            var tempElement = Head;
            // Перебираем все элементы
            while (tempElement.Next is not null)
                tempElement = tempElement.Next;
            // Добавляет новый элемент к последнему
            tempElement.Next = newListElement;
            // Вовзращаем ссылку на новосозданную ноду
            return newListElement;
        }

        /// <summary>
        ///     Возвращает элемент по индексу в списке.
        /// </summary>
        /// <param name="index">Индекс возвращаемого элемента.</param>
        /// <returns>Элемент по индексу</returns>
        private T GetElementByIndex(int index)
        {
            // Индекс не может быть отрецательным
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            // Создаём временну ссылку на голову
            var tempElement = Head;

            // Итерируем список, пока не найдём наш элемент
            for (var i = 0; tempElement is not null && i < index; i++)
                tempElement = tempElement.Next;

            // Если временный указатель указывает в никуда, то элемент не нашелся
            if (tempElement is null)
                throw new ArgumentOutOfRangeException(nameof(index));

            // Иначе возвращаем элемент, который нашли
            return tempElement.Data;
        }

        /// <summary>
        ///     Длина списка
        /// </summary>
        /// <returns>Длина списка</returns>
        public int GetLength()
        {
            // Если голова пустая, то список пустой
            if (Head is null)
                return 0;
            // Создаём временную ссылку на голову
            var tempElement = Head;
            // Длина как минимум равна 1
            var length = 1;

            // Итерируем каждый элемент
            while (tempElement.Next is not null)
            {
                tempElement = tempElement.Next;
                length++;
            }

            // Возвращаем длину списка
            return length;
        }

        /// <summary>
        ///     Удаление элемента в списке
        /// </summary>
        /// <param name="element">Элемент, который мы хотим удалить</param>
        /// <returns>Результат операции удаления</returns>
        public bool DeleteElement(T element)
        {
            var currentElement = Head;
            SinglyLinkedListNode previousElement = null;

            // итерируем каждый элемент
            while (currentElement is not null)
            {
                // Проверяем, если элемент, который должен быть удалён
                // находится в этом списке
                if (currentElement.Data is null
                    && element is null
                    || currentElement.Data is not null
                    && currentElement.Data.Equals(element))
                {
                    // если элемент находится в голове, то просто
                    // переставляем голову на следующую ноду
                    if (currentElement.Equals(Head))
                    {
                        Head = Head.Next;
                        return true;
                    }

                    // иначе берём предыдущий элемент и перезаписываем
                    // следующий с тем, что был удалён
                    if (previousElement is not null)
                    {
                        previousElement.Next = currentElement.Next;
                        return true;
                    }
                }

                // Итерируем следущую ноду
                previousElement = currentElement;
                currentElement = currentElement.Next;
            }

            return false;
        }

        /// <summary>
        ///     Копирование части списка
        /// </summary>
        /// <param name="from">От куда начинаем копировать</param>
        /// <param name="to">До куда начинаем копировать</param>
        /// <param name="step">Шаг добавление (по умолчанию - 1)</param>
        /// <returns>Новый список (слайс)</returns>
        public SinglyLinkedList<T> Copy(int from, int to, int step = 1)
        {
            // Создаём список, в котором будет храниться слайс
            var result = new SinglyLinkedList<T>();
            // Начиная с from, зачанчивая to, с шагом step добавляем в слайс
            // элемент исходного списка
            for (var i = from; i < to; i += step)
                result.AddLast(this[i]);
            // Возвращаем слайс
            return result;
        }

        /// <summary>
        ///     Объединие списков с помощью создания нового списка
        /// </summary>
        /// <param name="list">
        ///     Список, который необходимо вставить в конец
        ///     исходного списка
        /// </param>
        public void MergeByCreatingNewList(SinglyLinkedList<T> list)
        {
            // Создаём промежуточный список, который будет хранить все элементы
            var result = new SinglyLinkedList<T>();
            // Каждый элемент исходного списка добавляем в промежуточный
            // список, а так же удаляем этот же элемент из исходного
            foreach (var item in this)
            {
                result.AddLast(item);
                DeleteElement(item);
            }

            // Для каждого элемента списка list добавляем в промежуточный
            // список
            foreach (var item in list)
                result.AddLast(item);
            // В исходный список добавляем элементы промежуточного списка
            foreach (var item in result)
                AddLast(item);
        }

        /// <summary>
        ///     Объединие списков без создания нового списка
        /// </summary>
        /// <param name="list">
        ///     Список, который необходимо вставить в конец
        ///     исходного списка
        /// </param>
        public void MergeWithoutCreatingNewList(SinglyLinkedList<T> list)
        {
            // Для каждого элемента списка list добавляем в исходный список
            foreach (var item in list)
                AddLast(item);
        }

        /// <summary>
        ///     Строит список в виде строки
        /// </summary>
        /// <returns>список в виде строки</returns>
        public override string ToString()
        {
            // Объявляем изменяемую строку
            var result = new StringBuilder();
            // для каждого элемента списка добавляем в нашу результирующую
            // строку элемент
            foreach (var item in this)
                result.Append(item).Append(' ');
            // возвращаем результирующую строку
            return result.ToString();
        }

        /// <summary>
        ///     Определение перечисления для итерации в foreach
        /// </summary>
        /// <returns>Перечисление</returns>
        public IEnumerator<T> GetEnumerator()
        {
            // временный ListElement, чтобы избежать перезаписи оригинала
            var tempElement = Head;

            // перебираем все элементы
            while (tempElement is not null)
            {
                // создаём перечисление для итерации в foreach
                yield return tempElement.Data;
                // смотрим на следующий элемент
                tempElement = tempElement.Next;
            }
        }

        /// <summary>
        ///     Нода для списка
        /// </summary>
        public class SinglyLinkedListNode
        {
            /// <summary>
            ///     Конструктор ноды
            /// </summary>
            /// <param name="data">Данные для ноды</param>
            public SinglyLinkedListNode(T data = default)
            {
                Data = data;
                Next = null;
            }

            /// <summary>
            ///     Данные ноды
            /// </summary>
            public T Data { get; }

            /// <summary>
            ///     Геттер и сеттер для следующего элемента
            /// </summary>
            public SinglyLinkedListNode Next { get; set; }
        }
    }

    public static class Program
    {
        public static void Main()
        {
            var rnd = new Random();

            // Инициализируем списки
            var firstList = new SinglyLinkedList<int>();
            var secondList = new SinglyLinkedList<int>();

            // Заполняем списки
            for (var i = 0; i < 10; i++)
            {
                firstList.AddFirst(rnd.Next(100));
                secondList.AddLast(rnd.Next(100));
            }

            Console.WriteLine($"Первый, после инициализации: {firstList}");
            Console.WriteLine($"Второй, после инициализации: {secondList}");

            // Удаляем элементы
            for (var i = 0; i < 5; i++)
            {
                firstList.DeleteElement(firstList[rnd.Next(firstList.GetLength())]);
                secondList.DeleteElement(secondList[rnd.Next(secondList.GetLength())]);
            }

            Console.WriteLine($"Первый, после удаления: {firstList}");
            Console.WriteLine($"Второй, после удаления: {secondList}");

            // Генерируем случайные границы, в которым будем делать слайс
            var (from, to) = (rnd.Next(5), rnd.Next(5));
            var thirdList = firstList.Copy(Math.Min(from, to), Math.Max(from, to));
            Console.WriteLine(
                $"Третий, после копирования из первого с {Math.Min(from, to)} до {Math.Max(from, to)}: {thirdList}");

            // Слияние двух списков двумя способами
            firstList.MergeByCreatingNewList(secondList);
            secondList.MergeWithoutCreatingNewList(thirdList);
            Console.WriteLine($"Первый, после слияние с вторым: {firstList}");
            Console.WriteLine($"Второй, после слиятния с третьим: {secondList}");
        }
    }
}
