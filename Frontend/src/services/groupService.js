// datos de ejemplo
const groups = [
  {
    id: '123',
    name: 'lk/Computer Science',
    author: 'Escuela Profesional de Ciencia de la Computación – UNSA',
    date: '28 de abril',
    content: `¡Oportunidad de especialización!
Ya están abiertas las inscripciones para el programa de MAESTRÍA EN CIENCIA DE DATOS en la UNSA/Arequipa. Los interesados podrán postular hasta el 18 de mayo. La carrera está dirigida a graduados en cualquier área del conocimiento...
#UNSA #Maestrías #CienciaDeLaComputación #CienciaDeDatos #Arequipa`,
    imageUrl: '/assets/data-science.jpg',  // o en public/
    likes: 102,
    comments: 4,
    shareCount: 1,
    // panel derecho
    groupInfo: {
      title: 'Computer Science',
      description: 'La Escuela Profesional de Ciencia de la Computación tiene por objetivo formar profesionales aptos para…',
      created: '1 de enero del 2025',
      visibility: 'Público',
      members: 339,
      online: 9
    }
  }
]
export function fetchGroup(id) {
  return new Promise(resolve => {
    const g = groups.find(x => x.id === id)
    setTimeout(() => resolve(g), 200)
  })
}
export function joinGroup(id) {
  return new Promise(resolve => {
    setTimeout(() => resolve({ success: true }), 200)
  })
}
