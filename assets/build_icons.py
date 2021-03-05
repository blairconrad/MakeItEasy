try:
    import Image
except:  # NOQA - fallback, no matter the cause
    try:
        from PIL import Image
    except:  # NOQA - we'll raise a better exception
        raise Exception("Can't import PIL or PILLOW. Install one.")


original = Image.open("makeiteasy_original.png")
cropped = original.crop((63, 11, 631, 579))
for width in (512, 256, 128, 64, 48, 32, 16):
    resized = cropped.resize((width, width), Image.BICUBIC)
    resized.save(f"makeiteasy_{width}.png")
social = Image.new("RGB", (cropped.size[1] * 2, cropped.size[1]), "white")
social.paste(cropped, (0, 0))
social = social.resize((640, 320), Image.BICUBIC)
social.save("social-preview.png")
