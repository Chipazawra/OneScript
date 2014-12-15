﻿
Перем юТест;

Функция ПолучитьСписокТестов(ЮнитТестирование) Экспорт
	
	юТест = ЮнитТестирование;
	
	ВсеТесты = Новый Массив;
	
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеБулево");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеРазмерностиЧисло");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеРазделительДробей");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеГруппировкаРазрядов");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеЛидирующиеНули");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеПредставлениеНуля");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеПредставлениеОтрицательных");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеПустаяДата");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеЛокальнаяДата");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеЛокальнаяДатаВремя");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеДлинныйФорматЛокальнойДаты");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеЛокальноеВремя");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьПроизвольноеФорматированиеДат");
	ВсеТесты.Добавить("ТестДолжен_ПроверитьФорматированиеВсехДатИзПериода");
	
	Возврат ВсеТесты;
КонецФункции

Процедура ТестДолжен_ПроверитьФорматированиеБулево() Экспорт
	
	Да = Истина;
	Нет = Ложь;
	
	юТест.ПроверитьРавенство("Ага", Формат(Да, "БИ='Ага'; БЛ = 'Ненене'"));
	юТест.ПроверитьРавенство("Ага", Формат(Истина, "БИ='Ага'; БЛ = 'Ненене'"));
	юТест.ПроверитьРавенство("Ненене", Формат(Нет, "БИ='Ага'; БЛ = 'Ненене'"));
	юТест.ПроверитьРавенство("Ненене", Формат(Ложь, "БИ='Ага'; БЛ = 'Ненене'"));
	
	юТест.ПроверитьРавенство("Да", Формат(Да, "ЧГ=0"));
	юТест.ПроверитьРавенство("Нет", Формат(Нет, "ЧГ=0"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеРазмерностиЧисло() Экспорт
	
	Целое = 150;
	Дробь = 123.1234;
	Большое = 123456777;
	
	юТест.ПроверитьРавенство("150", Формат(Целое, "ЧЦ=5; ЧДЦ=2"));
	юТест.ПроверитьРавенство("123.12", Формат(Дробь, "ЧЦ=5; ЧДЦ=2; L=en_US"));
	юТест.ПроверитьРавенство("999.99", Формат(Большое, "ЧЦ=5; ЧДЦ=2; L=en_US"));
	юТест.ПроверитьРавенство("123 456 777", Формат(Большое, "ЧЦ=10; ЧДЦ=2"));
	юТест.ПроверитьРавенство(".999", Формат(123.123, "ЧЦ=3; ЧДЦ=3; L=en_US"));
	юТест.ПроверитьРавенство("123/12", Формат(Дробь, "ЧЦ=5; ЧДЦ=2; ЧРД='/'"));
	юТест.ПроверитьРавенство("123/1234", Формат(Дробь, "ЧРД='/'"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеРазделительДробей() Экспорт
	
	Дробь = 123.1234;
	
	юТест.ПроверитьРавенство("123/12", Формат(Дробь, "ЧЦ=5; ЧДЦ=2; ЧРД='/'"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеГруппировкаРазрядов() Экспорт
	
	юТест.ПроверитьРавенство("1234567", Формат(1234567, "ЧГ="));
	юТест.ПроверитьРавенство("1234567", Формат(1234567, "ЧГ=0"));
	юТест.ПроверитьРавенство("1 23 45 67", Формат(1234567, "ЧГ=2"));
	юТест.ПроверитьРавенство("11 23 4567", Формат(11234567, "ЧГ=4,2"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеЛидирующиеНули() Экспорт
	
	юТест.ПроверитьРавенство("0 000 123", Формат(123, "ЧЦ=7; ЧВН=1"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеПредставлениеНуля() Экспорт
	
	юТест.ПроверитьРавенство("", Формат(0, "ЧЦ=7"));
	юТест.ПроверитьРавенство("0", Формат(0, "ЧН=0;"));
	юТест.ПроверитьРавенство("НОЛЬ", Формат(0, "ЧН='НОЛЬ';"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеПредставлениеОтрицательных() Экспорт
	
	Число = -123234.811;
	
	юТест.ПроверитьРавенство("(123234.811)", Формат(Число, "ЧО=0; ЧГ=0; L=en_US"));
	юТест.ПроверитьРавенство("-123 234.811", Формат(Число, "ЧО=1; L=en_US"));
	юТест.ПроверитьРавенство("- 123 234.811", Формат(Число, "ЧО=2; L=en_US"));
	юТест.ПроверитьРавенство("123 234.811-", Формат(Число, "ЧО=3; L=en_US"));
	юТест.ПроверитьРавенство("123 234.811 -", Формат(Число, "ЧО=4; L=en_US"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеПустаяДата() Экспорт

	ПустаяДата = '00010101';
	
	юТест.ПроверитьРавенство("", Формат(ПустаяДата, "ЧГ=0"));
	юТест.ПроверитьРавенство("--", Формат(ПустаяДата, "ДП=--"));

КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеЛокальнаяДата() Экспорт

	Эталон = '20140207122517';
	Эталон2 = '20141217122517';
	
	юТест.ПроверитьРавенство("07.02.2014", Формат(Эталон, "ДЛФ=Д; Л=ru_RU"));
	юТест.ПроверитьРавенство("17.12.2014", Формат(Эталон2, "ДЛФ=Д; Л=ru_RU"));

КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеЛокальнаяДатаВремя() Экспорт

	Эталон = '20140207022517';
	Эталон2 = '20141217022517';
	
	юТест.ПроверитьРавенство("07.02.2014 2:25:17", Формат(Эталон, "ДЛФ=ДВ; Л=ru_RU"));
	юТест.ПроверитьРавенство("17.12.2014 2:25:17", Формат(Эталон2, "ДЛФ=ДВ; Л=ru_RU"));

КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеЛокальноеВремя() Экспорт

	Эталон = '20140207022517';
	
	юТест.ПроверитьРавенство("2:25:17", Формат(Эталон, "ДЛФ=В; Л=ru_RU"));

КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеДлинныйФорматЛокальнойДаты() Экспорт

	Эталон = '20140207022517';
	
	// внимание, дата и время разделены неразрывным пробелом
	юТест.ПроверитьРавенство("7 февраля 2014 г.", Формат(Эталон, "ДЛФ=ДД; Л=ru_RU"), "Только дата");
	юТест.ПроверитьРавенство("7 февраля 2014 г. 2:25:17", Формат(Эталон, "ДЛФ=ДДВ; Л=ru_RU"), "Дата время");

КонецПроцедуры

Процедура ТестДолжен_ПроверитьПроизвольноеФорматированиеДат() Экспорт
	
	Эталон = '20140207020805';
	Эталон2 = '20141217020805';
	
	юТест.ПроверитьРавенство("02:08:05", Формат(Эталон, "ДФ=чч:мм:сс"));
	юТест.ПроверитьРавенство("02:08:05", Формат(Эталон, "ДФ='чч:мм:сс'"));
	юТест.ПроверитьРавенство("2:8:5", Формат(Эталон, "ДФ=ч:м:с"));
	юТест.ПроверитьРавенство("*2-8-5*", Формат(Эталон, "ДФ=*ч-м-с*"));
	
	юТест.ПроверитьРавенство("07-02-14/02 08 05", Формат(Эталон, "ДФ='дд-ММ-гг/чч мм сс'"));
	
	юТест.ПроверитьРавенство("7-2-14", Формат(Эталон, "ДФ='д-М-гг'"));
	юТест.ПроверитьРавенство("17-12-14", Формат(Эталон2, "ДФ='д-М-гг'"));
	
	юТест.ПроверитьРавенство("7-фев-2014", Формат(Эталон, "ДФ='д-МММ-гггг'; Л=ru_RU"));
	юТест.ПроверитьРавенство("17-дек-2014", Формат(Эталон2, "ДФ='д-МММ-гггг'; Л=ru_RU"));
	
	юТест.ПроверитьРавенство("07-февраля-2014", Формат(Эталон, "ДФ='дд-ММММ-гггг'; Л=ru_RU"));
	юТест.ПроверитьРавенство("07 февраля 2014", Формат(Эталон, "ДФ='дд ММММ гггг'; Л=ru_RU"));
	юТест.ПроверитьРавенство("17-декабря-2014", Формат(Эталон2, "ДФ='дд-ММММ-гггг'; Л=ru_RU"));
	юТест.ПроверитьРавенство("17 декабря 2014", Формат(Эталон2, "ДФ='дд ММММ гггг'; Л=ru_RU"));
	
КонецПроцедуры

Процедура ТестДолжен_ПроверитьФорматированиеВсехДатИзПериода() //Экспорт
	Для Год = 2000 По 2011 Цикл
		Дни = Новый Соответствие();
		Дни.Вставить(1, 31);
		Если (Год % 100) = 0 или (Год % 100) % 4 = 0 Тогда
			Дни.Вставить(2, 29);
		Иначе
			Дни.Вставить(2, 28);
		КонецЕсли;
		Дни.Вставить(3, 31);
		Дни.Вставить(4, 30);
		Дни.Вставить(5, 31);
		Дни.Вставить(6, 30);
		Дни.Вставить(7, 31);
		Дни.Вставить(8, 31);
		Дни.Вставить(9, 30);
		Дни.Вставить(10, 31);
		Дни.Вставить(11, 30);
		Дни.Вставить(12, 31);
		
		Для Месяц = 1 По 12 Цикл
			КоличествоДней = Дни.Получить(Месяц);
			Для День = 1 По КоличествоДней Цикл
			КонецЦикла;
		КонецЦикла;
	КонецЦикла;
		
КонецПроцедуры
