# ASCII Art Generator
A CLI tool to generate ASCII art from image files

# Features
- Convert most image files to ASCII text
- Customizable output size
- Adjustable image properties, like brightness, saturation and contrast
- Alpha channel support
- Edge detection filter through the Sobel Operator
- Customisable palettes
- Create and output to a text file

# Usage
Run `ascii-art-gen [path-to-image-file]` to convert an image to ASCII.

### Arguments
There are also different arguments for modifying the output, which must be added after the image file path. They are listed below, or can be seen through running `ascii-art-gen --help`, which lists all options.
| Argument                        | Description                                                                             | Value type and default                                |
|---------------------------------|-----------------------------------------------------------------------------------------|-------------------------------------------------------|
| `-h`\|`--height` `[Value]`      | Set the height of the output text                                                       | Integer, default = `64`                               |
| `-w`\|`--width` `[Value]`       | Set the width of the output text                                                        | Integer, default = `64`                               |
| `-b`\|`--brightness` `[Value]`  | Multiply the brightness of the input image                                              | Decimal, default = `1.0`                              |
| `-c`\|`--contrast` `[Value]`    | Multiply the contrast of the input image                                                | Decimal, default = `1.0`                              |
| `-s`\|`--saturation` `[Value]`  | Multiply the saturation of the input image                                              | Decimal, default = `1.0`                              |
| `-a`\|`--use-alpha` `[Value]`   | Use the alpha channel of the input image                                                | `true` or `false`, default = `true`                   |
| `-e`\|`--edge-detect` `[Value]` | Use an edge detection filter over the image                                             | `true` or `false`, default = `false`                  |
| `-p`\|`--palette` `[Value]`     | Set palette used to create output text, from darkest character to lightest character    | Text or Filepath to text file, default = `@%#*+=-:. ` |
| `-f`\|`--file` `[Value]`        | Output to a text file                                                                   | Filepath, default = none                              |
| `--help`                        | List all options for the program. Must be ran before an input filepath in order to work |                                                       |

# Credits
[SkiaSharp](https://github.com/mono/SkiaSharp) image processing library
