#version 330 core
out vec4 FragColor;

in vec3 texCoord;
in vec3 outColor;

uniform sampler2D textures[16];

void main() 
{
	FragColor =  texture(textures[int(texCoord.x)], texCoord.yz) * vec4(outColor, 1.0);
}