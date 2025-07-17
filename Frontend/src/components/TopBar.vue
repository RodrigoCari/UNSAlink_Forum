<template>
  <header class="topbar">
    <input class="search" type="text" placeholder="Buscar en UNSAlink" />
    <div class="actions">
      <button class="icon">💬</button>
      <button class="create-btn">+ Crear</button>
      <button class="icon">👤</button>
    </div>
  </header>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

.topbar {
  position: sticky;
  top: 0;
  z-index: 10;
  margin-left: 24px; /* compensa el ancho de SideBar */
  /*width: calc(100% - 220px); /* evita solapamiento */
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 60px;
  padding: 0 1.5rem;
  background-color: #f9fafb;
  border-bottom: 1px solid #e0e0e0;
  font-family: 'Roboto', sans-serif;
}

.search {
  width: 300px;
  padding: 0.5rem 1rem;
  border: 1px solid #d1d5db;
  border-radius: 999px;
  font-size: 0.9rem;
  outline: none;
}

.actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.icon {
  background: transparent;
  border: none;
  font-size: 1.25rem;
  cursor: pointer;
}

.create-btn {
  background-color: #e5e7eb;
  border: none;
  padding: 0.4rem 1rem;
  border-radius: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.create-btn:hover {
  background-color: #d1d5db;
}
</style>