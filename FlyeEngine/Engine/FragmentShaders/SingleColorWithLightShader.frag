#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 ambient;
in vec3 diffuse;
in vec3 specular;
in float specularExp;
in float illumModel;

uniform vec3 lightPosition;
uniform vec3 lightColor;
uniform vec3 viewPosition;

void main()
{
    // Ambient
    float ambientStrength = 0.1;
    vec3 ambientLight = ambientStrength * ambient;

    // Diffuse
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPosition - FragPos);
    vec3 diffuse = max(dot(norm, lightDir), 0.0) * diffuse;

    // Specular
    vec3 viewDir = normalize(viewPosition - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), specularExp);
    vec3 specular = spec * specular;

    vec3 finalColor = (ambientLight + diffuse + specular);
    FragColor = vec4(finalColor, 1.0);
}