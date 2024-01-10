#version 330 core
out vec4 FragColor;

in vec2 texCoord;
in vec3 Color;

uniform sampler2D texture0;

void main() 
{
	FragColor =  texture(texture0, texCoord) * vec4(Color, 1.0);
}