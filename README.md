# Star Citizen Tools v0.6.0
[![Build](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml/badge.svg?branch=master&event=push)](https://github.com/h0useRus/StarCitizen/actions/workflows/build.yml)
[![Latest Release Version](https://img.shields.io/github/release/h0useRus/StarCitizen?sort=date)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Latest Release Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/latest/total)](https://github.com/h0useRus/StarCitizen/releases/latest)
[![Total Downloads](https://img.shields.io/github/downloads/h0useRus/StarCitizen/total.svg)](https://github.com/h0useRus/StarCitizen/releases)
[![MIT License](https://img.shields.io/apm/l/atomic-design-ui.svg?)](https://github.com/h0useRus/StarCitizen/blob/master/LICENSE)

[Русская версия](README_ru.md)  
[Українcька версія](README_uk.md)  

## Guides

- [Setup Application](guide/SETUP_APPLICATION.md)
- [Update Application](guide/UPDATE_APPLICATION.md)
- [Install Localization](guide/INSTALL_LOCALIZATION.md)
- [Update Localization](guide/UPDATE_LOCALIZATION.md)

## Features

### Localization features

- Download and install localization from GitHub repositories
- Select localization release version, language from provided by localization
- Disable localization without full uninstall
- Full unininstall localization (with cleanup from all files)
- Track and notify about available new versions of localization package
- Manage localization repositories
- Install localization archives from any folder (NEW in 0.6.0)
- Separate localization installation for PTU/LIVE
- Hide localization pre-release versions by default
- Specify GitHub auth token in settings json to access private repositories

### General features

- Move LIVE to PTU or reverse
- Manage user.cfg game settings (NEW in 0.6.0)
- Download and install application updates
- Supported UI languages: English, Russian, Ukrainian, Korean
- Track and notify about available new version of application
- Configurable application settings:
    - Auto run on system start (by default: OFF)
    - Launch minimized to system tray (by default: OFF)
    - Stay on top of other applications (by default: ON)
    - Enable use system proxy (by default: OFF)
    - Allow download application pre-release (by default: OFF)
