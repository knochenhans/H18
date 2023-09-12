import random
import string


def get_random_string(length):
    # choose from all lowercase letter
    letters = string.ascii_lowercase
    return ''.join(random.choice(letters) for i in range(length))


sprite_height = 26

# Number of sprites to generate
num_sprites = 80
frames_per_sheet = 5

# Starting x-coordinate value
start_x = 0

ext_resource_id = '5_d8ynp'

# Create a template for the sprite entry
sub_resource_template = '''
[sub_resource type="AtlasTexture" id="AtlasTexture_{}"]
atlas = ExtResource("{}")
region = Rect2({}, 0, 16, {})
'''

frame_template = '''{{
"duration": 1.0,
"texture": SubResource("AtlasTexture_{}")
}}'''

sheet_template = '''{{
"frames": [{}],
"loop": true,
"name": &"aim_left_{}",
"speed": 5.0
}}'''

frame_ids = []
sprites_output = ""
sheet_output = ""

# Resources
for i in range(num_sprites):
    x = start_x + i * 16
    frame_id = get_random_string(5)
    sprites_output += sub_resource_template.format(frame_id, ext_resource_id, x, sprite_height)
    frame_ids.append(frame_id)

current_frame_nr = 0

for i in range(int(num_sprites / frames_per_sheet)):
    frame_output = ""
    for f in range(frames_per_sheet):
        if frame_output != "":
            frame_output += ", "
        frame_output += frame_template.format(frame_ids[current_frame_nr])
        current_frame_nr += 1

    if sheet_output != "":
        sheet_output += ", "
    sheet_output += sheet_template.format(frame_output, i)

with open('output.json', 'w') as file:
    file.write(sprites_output)
    file.write(sheet_output)
