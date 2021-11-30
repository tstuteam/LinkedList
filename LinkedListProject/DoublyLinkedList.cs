/*
 * Copyright 2021 Andrey Malov
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedList
{
    /// <summary>
    ///     Реализация двусвязного списка
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public class DoublyLinkedList<T> where T : IComparable
    {
        /// <summary>
        ///     Этот класс используется для реализации узлов связанного списка.
        /// </summary>
        internal class Node
        {
            /// <summary>
            ///     Указатель на следующий узел
            /// </summary>
            public Node Next;

            /// <summary>
            ///     Указатель на предыдущий узел
            /// </summary>
            public Node Previous;

            /// <summary>
            ///     Значение узла
            /// </summary>
            public T Value;

            /// <summary>
            ///     Конструктор
            /// </summary>
            /// <param name="value">Значение данных узла</param>
            public Node(T value = default)
            {
                Value = value;
            }

            /// <summary>
            ///     Отображает узел
            /// </summary>
            /// <returns>Узел</returns>
            public override string ToString()
            {
                return Value.ToString();
            }
        }

        /// <summary>
        ///     Размер списка
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        ///     Указатель к началу списка
        /// </summary>
        private Node _head;

        /// <summary>
        ///     Указатель к концу списка
        /// </summary>
        private Node _tail;

        /// <summary>
        ///     Проверка на пустоту
        /// </summary>
        private bool IsEmpty => _head is null;

        /// <summary>
        ///     Конструктор
        /// </summary>
        public DoublyLinkedList()
        {
            _head = _tail = null;
            Size = 0;
        }

        /// <summary>
        ///     Вставка элемента в начало списка
        ///     Временная сложность: O(1)
        /// </summary>
        /// <param name="x"></param>
        public void PushFront(T x)
        {
            // Создаём новую ссылку с привязанным к ней значением
            Node newLink = new(x);
            // Если элемент - первый, то делаем ссылку и на конец списка
            if (IsEmpty)
                _tail = newLink;
            else
                _head.Previous = newLink; // newLink <-- текущая голова списка (head)
            newLink.Next = _head; // newLink <--> текущая голова списка (head)
            _head = newLink; // newLink (head) <--> старая голова списка
            ++Size;
        }

        /// <summary>
        ///     Вставка элемента в конец списка
        ///     Временная сложность: O(1)
        /// </summary>
        /// <param name="x"></param>
        public void PushBack(T x)
        {
            Node newLink = new(x)
            {
                Next = null // текущий хвост(tail)     newlink --> null
            };
            if (IsEmpty)
            {
                // Если элемент - первый, то ссылка должна указывать на начало и на конец списка
                _tail = newLink;
                _head = _tail;
            }
            else
            {
                _tail.Next = newLink; // текущий хвост (tail) --> newLink --> null
                newLink.Previous = _tail; // текущий хвост (tail) <--> newLink --> null
                _tail = newLink; // старый хвост <--> newLink (tail) --> null
            }

            // Увеличиваем размер списка
            ++Size;
        }

        /// <summary>
        ///     Вставка элемента с индексом.
        ///     Если на месте индекса вставочного элемента существует какой-то другой элемент, то мы сдвигаем его вправо.
        ///     Временная сложность: O(n/2), где n - размер списка
        /// </summary>
        /// <param name="x">Элемент, который мы хотим вставить в наш список</param>
        /// <param name="index">Индекс, который будет принадлежать нашему новому элементу</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void Insert(T x, int index)
        {
            if (index > Size || index < 0)
                throw new IndexOutOfRangeException("Index given: " + index + ", but size is: " + Size);

            var newNode = new Node(x);
            Node temp;

            if (index == 0)
            {
                PushFront(x);
                return;
            }

            if (index == Size)
            {
                PushBack(x);
                return;
            }

            if (index < Size / 2) // Вставляем в первую часть списка (первая половина списка)
            {
                temp = _head;
                // Находим нужный элемент, который надо сдвинуть
                for (var i = 0; i < index; i++)
                    temp = temp.Next;
            }
            else // Вставляем во вторую часть списка (вторая половина списка)
            {
                temp = _tail;
                // Находим нужный элемент, который надо сдвинуть
                for (var i = Size; i > index + 1; i--)
                    temp = temp.Previous;
            }

            // Делаем замены
            temp.Previous.Next = newNode;
            newNode.Previous = temp.Previous;
            newNode.Next = temp;
            temp.Previous = newNode;
            // Увеличиваем размер на 1
            Size++;
        }

        /// <summary>
        ///     Удаляем элемент с начала списка
        ///     Временная сложность: O(1)
        /// </summary>
        /// <returns></returns>
        public T PopFront()
        {
            var temp = _head;
            _head = _head.Next; // oldHead <--> 2ndElement(head)

            if (_head is null)
                _tail = null;
            else
                _head.Previous =
                    null; // oldHead --> 2ndElement(head) ничего не указывает на старую голову, поэтому будет удалено
            --Size;
            return temp.Value;
        }

        /// <summary>
        ///     Удаляем элемент с конца списка
        ///     Временная сложность: O(1)
        /// </summary>
        /// <returns></returns>
        public T PopBack()
        {
            var temp = _tail;
            _tail = _tail.Previous; // 2ndLast(tail) <--> oldTail --> null

            if (_tail is null)
                _head = null;
            else
                _tail.Next = null; // 2ndLast(tail) --> null
            --Size;
            return temp.Value;
        }

        /// <summary>
        ///     Удаляем элемент из списка
        ///     Временная сложность: O(n), где n - размер списка
        /// </summary>
        /// <param name="x">Элемент, который мы хотим удалить</param>
        /// <exception cref="ArgumentException">Если элемент отсутствует в списке</exception>
        public void Remove(T x)
        {
            var current = _head;
            // Удаляем элемент
            if (current == _head) // если он оказался головой
            {
                PopFront();
                return;
            }

            if (current == _tail) // если он оказался концом списка
            {
                PopBack();
                return;
            }

            // Находим элемент, который необходимо удалить
            while (!current.Value.Equals(x))
                if (current != _tail)
                    current = current.Next;
                else // Если доходим до хвоста, а элемент все еще не найден
                    throw new ArgumentException($"The element {x} to be deleted does not exist in the list!");
            // До удаления: 1 <--> 2(current) <--> 3
            // Удаляем
            current.Previous.Next = current.Next; // 1 --> 3
            current.Next.Previous = current.Previous; // 1 <--> 3
            --Size;
        }

        /// <summary>
        ///     Замена элемента x на место y
        /// </summary>
        /// <param name="x">Элемент, который заменит y</param>
        /// <param name="y">Элемент, который будет замещён</param>
        public void Replace(T x, T y)
        {
            Insert(x, IndexOf(y));
            Remove(y);
        }

        /// <summary>
        ///     Получение индекса элемента
        /// </summary>
        /// <param name="x">Элемент, индекс которого мы ищем</param>
        /// <returns>Индекс элемента</returns>
        public int IndexOf(T x)
        {
            // TODO(andreymlv): Доделать, не используя массив
            return Array.IndexOf(this.ToString().Split(' '), x.ToString());
        }

        /// <summary>
        ///     Определение перечисления для итерации в foreach
        /// </summary>
        /// <returns>Перечисление</returns>
        public IEnumerator<T> GetEnumerator()
        {
            // временный ListElement, чтобы избежать перезаписи оригинала
            var tempElement = _head;

            // перебираем все элементы
            while (tempElement is not null)
            {
                // создаём перечисление для итерации в foreach
                yield return tempElement.Value;
                // смотрим на следующий элемент
                tempElement = tempElement.Next;
            }
        }

        /// <summary>
        ///     Возвращает элемент по индексу в списке.
        /// </summary>
        /// <param name="key">Индекс возвращаемого элемента</param>
        /// <returns>Элемент по индексу</returns>
        public T this[int key] => GetElementByIndex(key);

        /// <summary>
        ///     Возвращает слайс из диапазона.
        /// </summary>
        /// <param name="range">Диапазон значений</param>
        public DoublyLinkedList<T> this[Range range]
        {
            get
            {
                var start = range.Start.Value;
                var end = range.End.Value;

                // [..]
                if (range.Start.Equals(0) && range.End.Equals(^0))
                    return Copy(0, Size);

                // list[start..]
                if (start > end)
                    return Copy(start, Size);

                // [start..end]
                return Copy(start, end);
            }
        }

        /// <summary>
        ///     Возвращает элемент по индексу в списке.
        /// </summary>
        /// <param name="index">Индекс возвращаемого элемента.</param>
        /// <returns>Элемент по индексу.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Индекс не может быть отрицательным или больше размера списка</exception>
        private T GetElementByIndex(int index)
        {
            // Индекс не может быть отрицательным или больше размера списка
            if (index < 0 || index > Size)
                throw new ArgumentOutOfRangeException(nameof(index));

            // Создаём временную ссылку на голову
            var tempElement = _head;

            // Итерируем список, пока не найдём наш элемент
            for (var i = 0; tempElement is not null && i < index; i++)
                tempElement = tempElement.Next;

            // Если временный указатель указывает в никуда, то элемент не нашелся
            if (tempElement is null)
                throw new ArgumentOutOfRangeException(nameof(index));

            // Иначе возвращаем элемент, который нашли
            return tempElement.Value;
        }

        /// <summary>
        ///     Копирование части списка
        ///     Временная сложность: O(n)
        /// </summary>
        /// <param name="from">От куда начинаем копировать</param>
        /// <param name="to">До куда начинаем копировать</param>
        /// <param name="step">Шаг добавление (по умолчанию - 1)</param>
        /// <returns>Новый список (слайс)</returns>
        public DoublyLinkedList<T> Copy(int from, int to, int step = 1)
        {
            // Создаём список, в котором будет храниться слайс
            var result = new DoublyLinkedList<T>();
            // Начиная с from, заканчивая to, с шагом step добавляем в слайс
            // элемент исходного списка
            for (var i = from; i < to; i += step)
                result.PushBack(this[i]);
            // Возвращаем слайс
            return result;
        }

        /// <summary>
        ///     Объедение списков с созданием нового списка
        ///     Временная сложность: O(n+m)
        /// </summary>
        /// <param name="list">
        ///     Список, который необходимо вставить в конец
        ///     исходного списка
        /// </param>
        public DoublyLinkedList<T> Merge(DoublyLinkedList<T> list)
        {
            // Создаём промежуточный список, который будет хранить все элементы исходного списка
            var result = this[..];
            // Делаем слияние промежуточного списка и списка list
            result.MergeWith(list);
            return result;
        }

        /// <summary>
        ///     Объедение списков без создания нового списка
        ///     Временная сложность: O(n)
        /// </summary>
        /// <param name="list">
        ///     Список, который необходимо вставить в конец
        ///     исходного списка
        /// </param>
        public void MergeWith(DoublyLinkedList<T> list)
        {
            // Для каждого элемента списка list добавляем в исходный список
            foreach (var item in list)
                PushBack(item);
        }

        /// <summary>
        ///     Печатает содержимое списка
        /// </summary>
        /// <returns>Содержимое списка</returns>
        public override string ToString()
        {
            // Объявляем изменяемую строку
            var result = new StringBuilder();
            // Делаем указатель на начало списока, чтобы не нарушить оригинальный список
            var current = _head;
            // Пока не дойдём до конца, добавляем в строку элемент узла
            while (current is not null)
            {
                result.Append(current).Append(' ');
                current = current.Next;
            }

            // возвращаем результативную строку
            return result.ToString();
        }
    }
}
