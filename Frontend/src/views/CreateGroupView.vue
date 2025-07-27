<template>
  <div class="creategroup-container">
    <div class="content">
      <h1 class="title">Crear Grupo</h1>
      <p class="message">Aún no te has unido a un grupo o comunidad</p>
      <button class="explore-btn">Visita r/Explorar</button>
    </div>
  </div>
</template>

<style scoped>
  @import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;600&display=swap');

  .creategroup-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background-color: #f3f4f6;
    font-family: 'Montserrat', sans-serif;
    margin-left: 245px;
  }

  .content {
    text-align: center;
    color: #2d2d2d;
  }

  .title {
    font-size: 1.75rem;
    font-weight: 600;
    margin-bottom: 1rem;
  }

  .message {
    font-size: 1rem;
    color: #666;
    margin-bottom: 1rem;
  }

  .explore-btn {
    background-color: #2d9cdb;
    color: white;
    border: none;
    padding: 0.6rem 1.5rem;
    border-radius: 20px;
    font-weight: 600;
    cursor: pointer;
    transition: background-color 0.3s;
  }

    .explore-btn:hover {
      background-color: #228ac7;
    }
</style>
