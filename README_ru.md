# Star Citizen Tools v0.7.0
[![Build](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml/badge.svg?branch=master&event=push)](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml)
[![Latest Release Version](https://img.shields.io/github/release/h0useRus/StarCitizen?sort=date)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Latest Release Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/latest/total)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Total Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/total.svg)](https://github.com/h0useRus/StarCitizen/releases)
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/h0useRus/StarCitizen/blob/master/LICENSE)

## Руководство

- [Настройка приложения](guide/SETUP_APPLICATION.md)
- [Обновление приложения](guide/UPDATE_APPLICATION.md)
- [Установка локализации](guide/INSTALL_LOCALIZATION.md)
- [Обновление локализации](guide/UPDATE_LOCALIZATION.md)

## Возможности 

### Локализация

- Скачивание и установка локализации из репозиториев GitHub
- Выбор версии выпуска локализации, языка из предоставленных локализацией
- Отключение локализации без полной деинсталляции
- Полная деинсталляции локализация (с очисткой от всех файлов)
- Отслеживание и уведомление о доступных новых версиях пакета локализации
- Управление репозиториями локализации
- Установка архивов локализации из любой папки (НОВИНКА в 0.6.0)
- Отдельная установка локализации для PTU / LIVE
- Скрытие предварительных версии локализации по умолчанию
- Возможность указать токен аутентификации GitHub для доступа к частным репозиториям (вручную через редактирование settings.json) 
- Умная докачка только изменений локализации (НОВИНКА в 0.6.1)
- Поддержка скачивать локализации и обновления приложения с Gitee для стран и регионов где заблокирован GitHub (НОВИНКА в 0.6.2)
- Проверка обновлений локализации раз в 7 дней на старте приложения (НОВИНКА в 0.6.3) 

### Общие

- Переименирование папки LIVE в PTU или обратно
- Управление настройками игры user.cfg (НОВИНКА в 0.6.0)
- Скачивание и установка обновлений приложения
- Проверка обновлений приложения раз в 7 дней на его старте (НОВИНКА в 0.6.3) 
- Поддерживаемые языки интерфейса: английский, русский, украинский, корейский.
- Отслеживание и уведомление о наличии новой версии приложения
- Настраиваемые параметры приложения:
     - Автоматический запуск при запуске системы (по умолчанию: Выключено)
     - Запуск свернутым в системный трей (по умолчанию: Выключено)
     - Показывать всегда сверху других окон (по умолчанию: Включено)
     - Использование системного прокси (по умолчанию: Выключено)
     - Загрузка предварительных версии приложения (по умолчанию: Выключено) 
