#version 330 core
out vec4 FragColor;

in vec3 texCoord;
in vec3 outColor;

uniform sampler2DArray textures;

void main() 
{
	FragColor = texture(textures, texCoord.yzx) * vec4(outColor, 1.0);
}