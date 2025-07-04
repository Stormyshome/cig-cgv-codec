# CRIC – Der verlustfreie Bild- und Videocodec

Willkommen beim offiziellen Repository des **CRIC-Codecs** – einem extrem schnellen, verlustfreien Bild- und Videokompressionsformat, optimiert für einfache Strukturen, Graustufenbilder und Streaming-Anwendungen.

## 🔧 Dateiendungen

- cig – CRIC Image Format (für Einzelbilder)
- cgv – CRIC Gray Video Format (für Videostreams oder Bildfolgen)

## 🚀 Vorteile

- 100 % **verlustfreie Rekonstruktion**
- Blitzschnelle Kompression & Dekompression (lineare Geschwindigkeit pro Pixel)
- Hervorragend geeignet für **Graustufenbilder** & einfache Farbdaten
- Ideal für **Live-Streaming**, **Embedded Systems**, **Machine Vision**
- Kein Huffman, keine Entropiekodierung – dadurch minimalste Latenz

## 📦 Downloads

Encoder & Decoder-Tools folgen in Kürze als Release.

## 📄 Dokumentation

Eine vollständige Beschreibung des Dateiformats findest du in `docs/spec.md` (wird laufend erweitert).

## 💬 Beispielverwendung (CLI – geplant)

```bash
cric-encode input.png output.cig
cric-decode input.cig output.png
cric-stream input_folder/ output.cgv
```

## 💰 Unterstützen

Gefällt dir dieses Projekt? Unterstütze die Entwicklung mit einer Spende ❤️

- [paypal.me/Stormyshome](https://paypal.me/Stormyshome)

![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)
## 📝 Lizenz

Dieses Projekt ist unter der **MIT-Lizenz** veröffentlicht – frei nutzbar, auch kommerziell. Siehe `LICENSE`.

---

Made with 💡 by **Chris**

