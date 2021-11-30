// Copyright 2021 Andrey Malov

using System;

namespace LinkedList
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Алгоритмы работы со списками. "
                              + "Методы класса Список: "
                              + "добавление "
                              + "удаление "
                              + "копирование части списка "
                              + "слияние двух списков(2 варианта: с и без создания нового списка) "
                              + "проверка на пустоту.\n");

            var rnd = new Random();

            // Инициализируем списки
            var firstList = new DoublyLinkedList<int>();
            var secondList = new DoublyLinkedList<int>();

            // Заполняем списки случайными числами
            for (var i = 0; i < 10; i++)
            {
                firstList.PushBack(rnd.Next(100));
                secondList.PushFront(rnd.Next(100));
            }

            Console.WriteLine($"Первый, после инициализации: {firstList}");
            Console.WriteLine($"Второй, после инициализации: {secondList}");

            // Удаляем элементы
            for (var i = 0; i < 5; i++)
            {
                firstList.Remove(firstList[rnd.Next(firstList.Size)]);
                secondList.Remove(secondList[rnd.Next(secondList.Size)]);
            }

            Console.WriteLine($"Первый, после удаления: {firstList}");
            Console.WriteLine($"Второй, после удаления: {secondList}");

            // Генерируем случайные границы, в которым будем делать слайс
            var (from, to) = (rnd.Next(5), rnd.Next(5));
            (from, to) = (Math.Min(from, to), Math.Max(from, to));

            // Копирование (слайс)
            var thirdList = firstList.Copy(from, to);
            Console.WriteLine($"Третий, после копирования из первого с {from} до {to}: {thirdList}");
            var fourthList = firstList[from..to];
            Console.WriteLine(
                $"Четвёртый, после такого же копирования, но с красивым синатксисом слайса: {fourthList}");

            // Слияние двух списков двумя способами
            var fifthList = firstList.Merge(secondList);
            secondList.MergeWith(thirdList);
            Console.WriteLine($"Пятый, после слияния первого со вторым: {fifthList}");
            Console.WriteLine($"Второй, после слиятния с третьим: {secondList}");
        }
    }
}
