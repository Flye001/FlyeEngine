#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 Color;

uniform vec3 lightPosition;
//uniform vec3 color;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPosition - FragPos);

    float diff = max(dot(norm, lightDir), 0.1);

    FragColor = vec4(Color * diff, 1.0);
}