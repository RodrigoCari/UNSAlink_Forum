// datos de ejemplo
const groups = [
  { id: '123', name: 'CS Fans', description: 'Grupo de entusiastas CS' },
  // …otros grupos
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
