#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform vec3 color;

void main() 
{
	FragColor =  texture(texture0, texCoord) * vec4(color, 1.0);
}