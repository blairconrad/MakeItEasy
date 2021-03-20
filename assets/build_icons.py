try:
    import Image
except:  # NOQA - fallback, no matter the cause
    try:
        from PIL import Image
    except:  # NOQA - we'll raise a better exception
        raise Exception("Can't import PIL or PILLOW. Install one.")


original = Image.open("makeiteasy_original.png")
cropped = original.crop((63, 11, 631, 579))
for background in ("white", "black", ""):
    for width in (512, 256, 128, 64, 48, 32, 16):
        resized = cropped.resize((width, width), Image.BICUBIC)

        filename = f"makeiteasy_{background}_{width}.png".replace("__", "_")
        icon = Image.new("RGBA", (width, width), background or "#00000000")
        icon.paste(resized, mask=resized)
        icon.save(filename)

        filename = f"makeiteasy_{background}_social_preview.png".replace("__", "_")

    social = Image.new(
        "RGBA", (cropped.size[1] * 2, cropped.size[1]), background or "#00000000"
    )
    social.paste(cropped, mask=cropped)
    social = social.resize((640, 320), Image.BICUBIC)
    social.save(filename)
