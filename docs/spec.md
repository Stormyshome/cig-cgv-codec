# CRIC Format – Technische Spezifikation (v0.1)

Dies ist die erste technische Spezifikation des verlustfreien Bild- und Videocodecs **CRIC**. Sie gilt für die Dateiformate:

- `.cig` (CRIC Image – Einzelbild)
- `.cgv` (CRIC Gray Video – Bildfolge/Stream)

---

## 🔧 Allgemeines Designprinzip
Der CRIC-Codec basiert auf einem extrem einfachen, linearen Konzept:
- Jeder Pixel wird einzeln analysiert
- Wenn alle Farbkanäle gleich sind, wird nur 1 Byte gespeichert
- Andernfalls werden alle Kanäle im Original gespeichert (z. B. RGB = 3 Bytes)
- Es werden keine Marker benötigt – die Byteanzahl impliziert die Bedeutung
- Erweiterung durch optionale Blöcke: RLE, Palette, gleichfarbige Blöcke

---

## 📄 Dateistruktur `.cig`

### Header (feste Länge, 12 Byte)
| Offset | Größe | Beschreibung                  |
|--------|-------|-------------------------------|
| 0      | 3     | Magic Bytes: `CIG`            |
| 3      | 1     | Version (z. B. `0x01`)        |
| 4      | 2     | Bildbreite (UInt16, LE)       |
| 6      | 2     | Bildhöhe (UInt16, LE)         |
| 8      | 1     | Farbkanäle: `3=RGB`, `4=RGBA` |
| 9      | 3     | Reserviert (0)                |

### Pixeldaten (variable Länge)
- Pro Pixel:
  - 1 Byte → wenn alle Kanäle gleich sind
  - `n` Bytes → wenn Kanäle verschieden sind (`n = 3` oder `4`)

---

## 📄 Dateistruktur `.cgv`

### Header (16 Byte)
| Offset | Größe | Beschreibung                  |
|--------|-------|-------------------------------|
| 0      | 3     | Magic Bytes: `CGV`            |
| 3      | 1     | Version (z. B. `0x01`)        |
| 4      | 2     | Bildbreite (UInt16, LE)       |
| 6      | 2     | Bildhöhe (UInt16, LE)         |
| 8      | 2     | Anzahl Kanäle pro Frame       |
| 10     | 2     | Framerate (z. B. `25`)        |
| 12     | 4     | Anzahl Frames (UInt32, LE)    |

### Datenblöcke:
- Jedes Frame wird wie `.cig` gespeichert (Header entfällt pro Frame)
- Optional: später Frame-Komprimierung oder Metadatenblock

---

## 📦 Erweiterung: Kompressionsmodi (optional)

### Marker:
- `250` = RLE
- `251` = Gleichfarbiger Block
- `252` = Palettenindex

Jeder Marker wird mit Folgebytes interpretiert (siehe Erweiterungsspezifikation, folgt).

---

## 📝 Hinweise
- Alle numerischen Werte im **Little-Endian** Format
- Farbreihenfolge: **R, G, B, [A]**
- Kompression ist vollständig **verlustfrei**
- Kodierung ist deterministisch und rein **linear** pro Pixel

---

Letzte Änderung: Juli 2025 – Entwurfsversion v0.1

