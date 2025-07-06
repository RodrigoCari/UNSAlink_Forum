<template>
  <div class="login-container">
    <div class="login-box">
      <div class="icon-lock">🔒</div>
      <h2>Welcome to Website</h2>
      <div class="input-group">
        <span class="icon">👤</span>
        <input type="text" placeholder="Username" />
      </div>
      <div class="input-group">
        <span class="icon">🔒</span>
        <input type="password" placeholder="Password" />
      </div>
      <button class="continue-btn">Continue</button>
      <p class="create-account">Create an account</p>
    </div>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Montserrat:wght@400;600&display=swap');

.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f3f4f6;
  font-family: 'Montserrat', sans-serif;
}

.login-box {
  background: #2d718f;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  width: 300px;
  text-align: center;
  color: white;
}

.icon-lock {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.input-group {
  display: flex;
  align-items: center;
  background: white; /* ✅ Fondo blanco */
  border: 1px solid #ccc;
  border-radius: 4px;
  margin: 0.5rem 0;
  padding: 0.5rem;
  color: black;
}

.input-group .icon {
  margin-right: 0.5rem;
  color: #2d718f;
}

.input-group input {
  border: none;
  outline: none;
  background: transparent;
  color: black; /* ✅ Texto negro */
  flex: 1;
  font-family: 'Montserrat', sans-serif;
}

.input-group input::placeholder {
  color: #888;
}

.continue-btn {
  background-color: #000000; /* ✅ Fondo negro */
  color: white;              /* ✅ Texto blanco */
  border: none;
  padding: 0.5rem;
  width: 100%;
  border-radius: 4px;
  margin-top: 1rem;
  cursor: pointer;
  font-weight: bold;
  font-family: 'Montserrat', sans-serif;
}

.create-account {
  margin-top: 1rem;
  color: rgba(255, 255, 255, 0.8);
  font-size: 0.875rem;
  cursor: pointer;
}
</style>
