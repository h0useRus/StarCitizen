# Star Citizen Tools v0.6.3
[![Build](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml/badge.svg?branch=master&event=push)](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml)
[![Latest Release Version](https://img.shields.io/github/release/h0useRus/StarCitizen?sort=date)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Latest Release Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/latest/total)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Total Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/total.svg)](https://github.com/h0useRus/StarCitizen/releases)
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/h0useRus/StarCitizen/blob/master/LICENSE)

## Посібники

- [Налаштування додатку](guide/SETUP_APPLICATION.md)
- [Оновлення додатку](guide/UPDATE_APPLICATION.md)
- [Встановлення локалізації](guide/INSTALL_LOCALIZATION.md)
- [Оновлення локалізації](guide/UPDATE_LOCALIZATION.md)

## Можливості

### Локалізація

- Завантаження і встановлення локалізації із GitHub репозиторіїв
- Вибір версії, мови із доступних для обраної локалізації
- Швидке вимкнення локалізації без повного видалення
- Повне видалення локалізації (з очисткою від усіх файлів)
- Відстежування і повідомлення про доступні нові версії пакетів локалізації
- Керування джерелами локалізації - можливість додавати власні, видалити стандартні, і т.п.
- Встановлення завантаженого вручну пакету локалізації з будь якої папки (НОВЕ у 0.6.0)
- Розділене встановлення локалізаці для PTU / LIVE
- Приховування по замовчуванню ранніх альфа версій локалізації
- Можливість задати токен GitHub для роботи з приватними репозиторіями (через ручне редагування settings.json) 
- Розумне довантаження локалізації без повного скачування (НОВЕ у 0.6.1)
- Підтримка завантаження локалізації і оновлень додатку з gitee для країн і регіонів де заблокований GitHub (НОВЕ у 0.6.2)
- Перевірка оновлень локалізації раз в 7 днів при запуску додатку (НОВЕ у 0.6.3)

### Загальні

- Перейменування папки LIVE у PTU чи навпаки
- Редагування налаштувань гри у user.cfg (НОВЕ у 0.6.0)
- Завантаження і встановлення оновлень додатку
- Перевірка оновлень додатку раз в 7 днів при його запуску (НОВЕ у 0.6.3)
- Підтримувані мови інтерфейсу: англійська, російська, українська, корейська
- Відстежування і повідомлення про доступні нові версії додатку
- Доступні додаткові налаштування:
    - Авто запуск на старті системи (по замовчуванню: Вимкнено)
    - Запускати сгорнутим у системний трей (по замовчуванню: Вимкнено)
    - Поверх усіх вікон (по замовчуванню: Увімкнено)
    - Використовувати системні проксі (по замовчуванню: Вимкнено)
    - Завантаження ранніх альфа версій додатку (по замовчуванню: Вимкнено)
