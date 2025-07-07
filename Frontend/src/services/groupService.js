// datos de ejemplo
const staticExtras = {
  '123': {
    content: `¡Oportunidad de especialización!
Ya están abiertas las inscripciones para el programa de MAESTRÍA EN CIENCIA DE DATOS en la UNSA/Arequipa. Los interesados podrán postular hasta el 18 de mayo. La carrera está dirigida a graduados en cualquier área del conocimiento...
#UNSA #Maestrías #CienciaDeLaComputación #CienciaDeDatos #Arequipa`,
    imageUrl: '/assets/data-science.jpg',
    likes: 102,
    comments: 4,
    shareCount: 1,
    groupInfo: {
      title: 'Computer Science',
      description: 'La Escuela Profesional de Ciencia de la Computación tiene por objetivo formar profesionales aptos para…',
      created: '1 de enero del 2025',
      visibility: 'Público',
      members: 339,
      online: 9
    }
  },
  '456': {
    content: `🎓 ¡Charla gratuita de introducción a Big Data!
Participa el próximo viernes 22 de mayo en el auditorio principal. Hablaremos de casos de uso en minería de datos y analítica predictiva.
#BigData #Analítica #Perú`,
    imageUrl: '/assets/big-data.jpg',
    likes: 76,
    comments: 12,
    shareCount: 3,
    groupInfo: {
      title: 'Data Analytics Peru',
      description: 'Grupo dedicado al análisis de datos, minería y BI en el contexto peruano.',
      created: '10 de febrero del 2025',
      visibility: 'Público',
      members: 215,
      online: 14
    }
  },
  '789': {
    content: `🚀 Tutorial: Despliegue de apps Node.js en Azure
Aprende paso a paso cómo configurar tu pipeline de CI/CD con GitHub Actions y el servicio de App Service de Azure.`,
    imageUrl: '/assets/fullstack.jpg',
    likes: 158,
    comments: 27,
    shareCount: 5,
    groupInfo: {
      title: 'Fullstack Developers',
      description: 'Encuentra recursos, tutoriales y soporte para desarrollo web front y back end.',
      created: '5 de marzo del 2025',
      visibility: 'Público',
      members: 482,
      online: 23
    }
  },
  '321': {
    content: `🔒 Vulnerabilidad crítica en OpenSSL
Se ha publicado un nuevo CVE que afecta a múltiples versiones. Revisen sus certificados y actualicen cuanto antes.
#Security #OpenSSL #CVE`,
    imageUrl: '/assets/cybersec.jpg',
    likes: 89,
    comments: 19,
    shareCount: 2,
    groupInfo: {
      title: 'Cybersecurity Hub',
      description: 'Foro para compartir alertas, herramientas y buenas prácticas en ciberseguridad.',
      created: '18 de enero del 2025',
      visibility: 'Privado',
      members: 128,
      online: 7
    }
  },
  '654': {
    content: `🤖 Hands-on: Redes neuronales con TensorFlow
Taller práctico este sábado 10 de mayo. Cupos limitados. ¡Regístrate ya!
#MachineLearning #TensorFlow #IA`,
    imageUrl: '/assets/ml.jpg',
    likes: 134,
    comments: 31,
    shareCount: 6,
    groupInfo: {
      title: 'Machine Learning Club',
      description: 'Espacio de discusión y práctica sobre algoritmos de aprendizaje automático.',
      created: '25 de febrero del 2025',
      visibility: 'Público',
      members: 310,
      online: 12
    }
  }
};

const API_BASE = 'https://localhost:44329/api/Group';

export async function fetchGroup(id) {
  const res = await fetch(`${API_BASE}/${id}`, { mode: 'cors' });
  if (!res.ok) throw new Error('Error al obtener el grupo');

  const api = await res.json();
  const extras = staticExtras[api.id] || {};

  return {
    id: api.id,
    name: api.name,
    description: api.description,
    author: api.adminName || `Admin ${api.adminId}`,
    date: new Date(api.creationDate)
      .toLocaleDateString('es-ES', { day: 'numeric', month: 'long' }),
    content: extras.content,
    imageUrl: extras.imageUrl,
    likes: extras.likes,
    comments: extras.comments,
    shareCount: extras.shareCount,
    groupInfo: extras.groupInfo
  };
}

export async function joinGroup(id) {
  const userId = '22222222-2222-2222-2222-222222222222';
  const res = await fetch(`${API_BASE}/${id}/join?userId=${userId}`, {
    method: 'POST',
    mode: 'cors'
  });
  if (!res.ok) throw new Error('Error al unirse al grupo');
  return res.json();
}
