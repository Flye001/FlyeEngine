#version 330 core
out vec4 FragColor;

in vec3 texCoord;
in vec3 vertColor;

uniform sampler2DArray textures;

void main() 
{
	FragColor = texture(textures, texCoord.yzx) * vec4(vertColor, 1.0);
}