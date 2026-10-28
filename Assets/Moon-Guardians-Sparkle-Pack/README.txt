Moon Guardians: Sparkle Transformation Pack - 5 guardian heroines + 15 spell effects + 20 decor pieces
by NogitsuneGurl - made in The Pixel Loom

Folders: Guardians/<piece>, Spells/<piece> and Decor/<piece>. Each piece has:
  frames/<piece>_on_00.png ...   transparent PNG frames at native size
  <piece>_sheet.png              all ON frames (Loom sheet layout)
  <piece>_preview.gif            4x browsing preview (use the PNGs in your game)
  <piece>_off.png                the unlit OFF look (lit decor only)
  game/                          engine strips: <piece>_on_strip.png, _off_strip.png,
                                 _on_glow_strip.png (glow pixels alone, lit decor), <piece>.anchor.json
                                 (frame size, timing, floor point, footprint, light)
  <piece>.pixel-loom-decor.json  the editable Loom project
  <piece>_shadow.png             solid #252b43 shadow sprite, same size (decor + characters;
                                 not flying pieces or spells); <piece>_on_shadow.png = matching strip
  <piece>_light_inner/middle/outer.png   light-layer masks for lit decor (flat greys 202/128/77,
                                 4-frame flicker strips, same frame size as the sprite)
Style: closed 1px pure-black outlines on characters and decor, flat clusters, no dithering, 3-5 steps per material,
Mistria 3/4 top-down decor (lit top planes), no floor shadow for wall/hanging/flying pieces,
top-left light, bases 4-6px above the frame bottom, frame sizes on the 8px grid.

Guardians (64x80 idle loops): Moon Guardian, Flame Guardian, Tide Guardian, Thunder Guardian, Love Guardian.
Spells (96x96 up to 136x80, 128x128 and 160x160, 16-24 frames): Sparkle Transformation, Crescent Wand Strike, Tiara Disc Toss, Ring of Sacred Fire, Bubble Mist Veil, Thunder Crash, Love Chain Heart Beam, Healing Moonlight, Rose Petal Storm, Wishing Star Shower, Masked Rose Knight Toss, Crescent Feather Healing, Planet Power Circle, Cosmic Rainbow Finale, Heart Compact Transformation.
Animated + ON/OFF lit decor: Transformation Brooch Stand, Crescent Moon Wand Display, Moon Crystal Palace, Fire Shrine & Torii Gate, Arcade Game Cabinet, Crescent Moon Lamp, Moon-Phase Window, Star Locket Music Box, Moonlight Vanity Mirror, Guardian Pen Set, Crystal Future City Skyline.
Animated decor (no light): Crescent Black Cat, Crescent White Cat, Guardian Ribbon Bunting, Winged Moon Horse, Ice-Cream Parlor Counter.
Still decor: Red Rose Vase, Heart Cushion, Moon Bunny Plush, Starry Bunny Bed.
Bonus variant: Guardians Group Pose (all five guardians together, 192x112 idle loop), in Variants/.
Some spells use soft half-transparent pixels (mist, moonlight); keep the PNGs for those, GIFs flatten them.
Canvas sizes and timings vary; see each anchor.json. You don't need The Pixel Loom to use the PNGs.
