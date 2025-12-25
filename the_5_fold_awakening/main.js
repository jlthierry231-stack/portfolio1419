// The 5 Fold Awakening - First Person Adventure Game
// Combining magic and technology: Star Wars meets Harry Potter

// Three.js setup
const scene = new THREE.Scene();
scene.background = new THREE.Color(0x000011); // Dark space-like

const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
camera.position.set(0, 1.6, 5); // Eye level

const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFSoftShadowMap;
document.getElementById('gameContainer').appendChild(renderer.domElement);

// Lighting
const ambientLight = new THREE.AmbientLight(0x404040, 0.4);
scene.add(ambientLight);

const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
directionalLight.position.set(10, 10, 5);
directionalLight.castShadow = true;
scene.add(directionalLight);

// Ground
const groundGeometry = new THREE.PlaneGeometry(100, 100);
const groundMaterial = new THREE.MeshLambertMaterial({ color: 0x228B22 });
const ground = new THREE.Mesh(groundGeometry, groundMaterial);
ground.rotation.x = -Math.PI / 2;
ground.receiveShadow = true;
scene.add(ground);

// Some objects: a magic crystal (tech-magic hybrid)
const crystalGeometry = new THREE.OctahedronGeometry(1);
const crystalMaterial = new THREE.MeshPhongMaterial({ color: 0x00ffff, emissive: 0x002222 });
const crystal = new THREE.Mesh(crystalGeometry, crystalMaterial);
crystal.position.set(5, 1, -5);
crystal.castShadow = true;
scene.add(crystal);

// A droid/sphere
const droidGeometry = new THREE.SphereGeometry(0.5);
const droidMaterial = new THREE.MeshLambertMaterial({ color: 0xff0000 });
const droid = new THREE.Mesh(droidGeometry, droidMaterial);
droid.position.set(-5, 0.5, -10);
droid.castShadow = true;
scene.add(droid);

// Controls
const controls = {
  moveForward: false,
  moveBackward: false,
  moveLeft: false,
  moveRight: false,
  canJump: false,
  velocity: new THREE.Vector3(),
  direction: new THREE.Vector3(),
};

document.addEventListener('keydown', (event) => {
  switch (event.code) {
    case 'ArrowUp':
    case 'KeyW':
      controls.moveForward = true;
      break;
    case 'ArrowLeft':
    case 'KeyA':
      controls.moveLeft = true;
      break;
    case 'ArrowDown':
    case 'KeyS':
      controls.moveBackward = true;
      break;
    case 'ArrowRight':
    case 'KeyD':
      controls.moveRight = true;
      break;
    case 'Space':
      if (controls.canJump) controls.velocity.y += 10;
      controls.canJump = false;
      break;
    case 'KeyE':
      advanceStory();
      break;
  }
});

document.addEventListener('keyup', (event) => {
  switch (event.code) {
    case 'ArrowUp':
    case 'KeyW':
      controls.moveForward = false;
      break;
    case 'ArrowLeft':
    case 'KeyA':
      controls.moveLeft = false;
      break;
    case 'ArrowDown':
    case 'KeyS':
      controls.moveBackward = false;
      break;
    case 'ArrowRight':
    case 'KeyD':
      controls.moveRight = false;
      break;
  }
});

// Mouse look
let mouseX = 0, mouseY = 0;
document.addEventListener('mousemove', (event) => {
  mouseX = (event.clientX / window.innerWidth) * 2 - 1;
  mouseY = -(event.clientY / window.innerHeight) * 2 + 1;
  camera.rotation.y = mouseX * Math.PI * 0.1;
  camera.rotation.x = mouseY * Math.PI * 0.1;
});

// Game variables
let health = 100;
let mana = 50;
const inventory = ['Wand', 'Blaster'];

let storyIndex = 0;
const storyTexts = [
  "Chapter 1: The Child With All Affinities and No Power\n\nYou are Kael Ardin, a 10-year-old boy with dormant magic for all five elements. Despite training, you can't access any power. Your friend Lyra comforts you after another failed test.",
  "Chapter 2: Academy of Sparks\n\nYou attend the futuristic magic academy. Holo-construct tutors teach spell theory, but students mock you for being 'Zero-Spark.'",
  "Chapter 3: A Friend Like No Other\n\nLyra stands up to bullies, showing her loyalty. She hints at a secret of her own.",
  "Chapter 4: Scanners Can’t Explain It\n\nA full-spectrum scan confirms your top 0.01% potential, but zero output. Confusion reigns.",
  "Chapter 5: Training Drones and Wooden Swords\n\nYou excel in non-magical combat, surprising instructors. Lyra trains with you.",
  "Chapter 6: The Lesson of Endurance\n\nPushing too hard in a simulation, you get hurt. Lyra carries you to the infirmary.",
  "Chapter 7: Rumors of Dark Mages\n\nNews of Dark Mages spreads. Security tightens at the academy.",
  "Chapter 8: The Festival of Lights\n\nAt the celebration, Lyra wins an award. She reassures you with a kiss on the cheek.",
  "Chapter 9: The Night the Vision Came\n\nYou collapse and enter a spirit realm vision.",
  "Chapter 10: The Ancient Archmage Appears\n\nArchmage Elion reveals your destiny: You are the Fivefold Mage, destined to stop Malakar the Voidborn."
];

function showStory() {
  const storyDiv = document.getElementById('story');
  storyDiv.textContent = storyTexts[storyIndex];
  storyDiv.style.display = 'block';
}

function hideStory() {
  document.getElementById('story').style.display = 'none';
}

function advanceStory() {
  storyIndex = (storyIndex + 1) % storyTexts.length;
  if (storyIndex === 0) {
    hideStory();
  } else {
    showStory();
  }
}

// Show initial story on load
showStory();

function updateHUD() {
  document.getElementById('health').textContent = `Health: ${health}`;
  document.getElementById('mana').textContent = `Mana: ${mana}`;
  document.getElementById('inventory').textContent = `Inventory: ${inventory.join(', ')}`;
}

// Animation loop
function animate() {
  requestAnimationFrame(animate);

  // Movement
  controls.direction.z = Number(controls.moveForward) - Number(controls.moveBackward);
  controls.direction.x = Number(controls.moveRight) - Number(controls.moveLeft);
  controls.direction.normalize();

  if (controls.moveForward || controls.moveBackward) camera.translateZ(-controls.direction.z * 0.1);
  if (controls.moveLeft || controls.moveRight) camera.translateX(-controls.direction.x * 0.1);

  // Gravity (simple)
  controls.velocity.y -= 0.01;
  camera.position.y += controls.velocity.y * 0.01;
  if (camera.position.y < 1.6) {
    camera.position.y = 1.6;
    controls.velocity.y = 0;
    controls.canJump = true;
  }

  // Animate objects
  crystal.rotation.x += 0.01;
  crystal.rotation.y += 0.01;
  droid.rotation.y += 0.02;

  renderer.render(scene, camera);
  updateHUD();
}

animate();

// Handle window resize
window.addEventListener('resize', () => {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
});

// Click to interact (cast spell or shoot)
document.addEventListener('click', (event) => {
  // Simple interaction: reduce mana, add effect
  if (mana > 10) {
    mana -= 10;
    // Create a magic bolt
    const boltGeometry = new THREE.SphereGeometry(0.1);
    const boltMaterial = new THREE.MeshBasicMaterial({ color: 0xffff00 });
    const bolt = new THREE.Mesh(boltGeometry, boltMaterial);
    bolt.position.copy(camera.position);
    bolt.position.add(camera.getWorldDirection(new THREE.Vector3()).multiplyScalar(2));
    scene.add(bolt);
    // Animate bolt
    const direction = camera.getWorldDirection(new THREE.Vector3());
    setTimeout(() => {
      bolt.position.add(direction.multiplyScalar(0.5));
    }, 100);
    setTimeout(() => scene.remove(bolt), 1000);
  }
});