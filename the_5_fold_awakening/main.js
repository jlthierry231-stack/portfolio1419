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

// Walls for a room
const wallGeometry = new THREE.PlaneGeometry(20, 10);
const wallMaterial = new THREE.MeshLambertMaterial({ color: 0x8B4513 });
const wall1 = new THREE.Mesh(wallGeometry, wallMaterial);
wall1.position.set(0, 5, -10);
scene.add(wall1);
const wall2 = new THREE.Mesh(wallGeometry, wallMaterial);
wall2.position.set(10, 5, 0);
wall2.rotation.y = Math.PI / 2;
scene.add(wall2);
const wall3 = new THREE.Mesh(wallGeometry, wallMaterial);
wall3.position.set(-10, 5, 0);
wall3.rotation.y = Math.PI / 2;
scene.add(wall3);

// Bed
const bedGeometry = new THREE.BoxGeometry(3, 0.5, 2);
const bedMaterial = new THREE.MeshLambertMaterial({ color: 0x654321 });
const bed = new THREE.Mesh(bedGeometry, bedMaterial);
bed.position.set(0, 0.25, 5);
bed.castShadow = true;
scene.add(bed);

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

// Trees or pillars
for (let i = 0; i < 5; i++) {
  const treeGeometry = new THREE.CylinderGeometry(0.5, 0.5, 5);
  const treeMaterial = new THREE.MeshLambertMaterial({ color: 0x8B4513 });
  const tree = new THREE.Mesh(treeGeometry, treeMaterial);
  tree.position.set((Math.random() - 0.5) * 40, 2.5, (Math.random() - 0.5) * 40);
  tree.castShadow = true;
  scene.add(tree);
}

// Enhanced world building: Tech buildings and magic elements
function createTechBuilding(x, z, height, color) {
  const geometry = new THREE.BoxGeometry(4, height, 4);
  const material = new THREE.MeshPhongMaterial({ color: color, emissive: color === 0x0000ff ? 0x000022 : 0 });
  const building = new THREE.Mesh(geometry, material);
  building.position.set(x, height/2, z);
  building.castShadow = true;
  building.receiveShadow = true;
  scene.add(building);

  // Add holographic effect
  if (color === 0x0000ff) {
    const holoGeometry = new THREE.PlaneGeometry(3, 3);
    const holoMaterial = new THREE.MeshBasicMaterial({ color: 0x00ffff, transparent: true, opacity: 0.3 });
    const hologram = new THREE.Mesh(holoGeometry, holoMaterial);
    hologram.position.set(x, height + 1, z);
    hologram.rotation.x = -Math.PI / 2;
    scene.add(hologram);
  }
}

createTechBuilding(15, -10, 8, 0x666666);
createTechBuilding(-15, -15, 6, 0x0000ff); // Blue tech building with hologram
createTechBuilding(20, 5, 10, 0x888888);

// Magic portals/crystals
function createMagicCrystal(x, y, z, element) {
  const geometry = new THREE.OctahedronGeometry(0.8);
  let color, emissive;
  switch(element) {
    case 'fire': color = 0xff4500; emissive = 0x441100; break;
    case 'water': color = 0x00ffff; emissive = 0x002222; break;
    case 'earth': color = 0x228B22; emissive = 0x114411; break;
    case 'air': color = 0xffffff; emissive = 0x222222; break;
    case 'spirit': color = 0xff69b4; emissive = 0x441122; break;
  }
  const material = new THREE.MeshPhongMaterial({ color: color, emissive: emissive, shininess: 100 });
  const crystal = new THREE.Mesh(geometry, material);
  crystal.position.set(x, y, z);
  crystal.castShadow = true;
  crystal.element = element;
  scene.add(crystal);
  return crystal;
}

const fireCrystal = createMagicCrystal(8, 1, -8, 'fire');
const waterCrystal = createMagicCrystal(-8, 1, -12, 'water');
const earthCrystal = createMagicCrystal(12, 1, 8, 'earth');
const airCrystal = createMagicCrystal(-12, 1, 12, 'air');
const spiritCrystal = createMagicCrystal(0, 1, -15, 'spirit');

// NPCs: Lyra and parents
function createNPC(x, z, color, name) {
  const geometry = new THREE.CylinderGeometry(0.3, 0.3, 1.6);
  const material = new THREE.MeshLambertMaterial({ color: color });
  const npc = new THREE.Mesh(geometry, material);
  npc.position.set(x, 0.8, z);
  npc.castShadow = true;
  npc.name = name;
  scene.add(npc);
  return npc;
}

const lyra = createNPC(3, -3, 0xff69b4, 'Lyra');
const mother = createNPC(-18, 2, 0xffdab9, 'Mother');
const father = createNPC(-12, 2, 0x8B4513, 'Father');

// Academy area
const academyGeometry = new THREE.BoxGeometry(12, 6, 12);
const academyMaterial = new THREE.MeshPhongMaterial({ color: 0x4169E1, emissive: 0x001122 });
const academy = new THREE.Mesh(academyGeometry, academyMaterial);
academy.position.set(25, 3, 15);
academy.castShadow = true;
academy.receiveShadow = true;
scene.add(academy);

// Holo-tutors
for (let i = 0; i < 3; i++) {
  const holoGeometry = new THREE.CylinderGeometry(0.2, 0.2, 2);
  const holoMaterial = new THREE.MeshBasicMaterial({ color: 0x00ffff, transparent: true, opacity: 0.6 });
  const holoTutor = new THREE.Mesh(holoGeometry, holoMaterial);
  holoTutor.position.set(25 + (i-1)*3, 1, 15);
  scene.add(holoTutor);
}

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

// Settings
let settings = {
  invertY: false,
  sensitivity: 1,
  showFPS: false,
  volume: 0.5,
  shadows: true
};

// Load settings from localStorage
if (localStorage.getItem('gameSettings')) {
  settings = JSON.parse(localStorage.getItem('gameSettings'));
}

// Apply initial settings
function applySettings() {
  document.getElementById('fpsDisplay').style.display = settings.showFPS ? 'block' : 'none';
  renderer.shadowMap.enabled = settings.shadows;
}

applySettings();

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
  camera.rotation.y = mouseX * Math.PI * 0.1 * settings.sensitivity;
  camera.rotation.x = (settings.invertY ? mouseY : -mouseY) * Math.PI * 0.1 * settings.sensitivity;
});

// Game variables
let health = 100;
let mana = 50;
let stamina = 100;
const inventory = ['Wand', 'Blaster'];

let inCutscene = true;
let cutsceneStep = 0;

// Initial camera position for cutscene (lying on bed)
camera.position.set(0, 0.5, 5);
camera.rotation.set(0, 0, 0);

let storyIndex = 0;
let currentChapter = 1;
let objectives = [
  "Explore your home and interact with your parents",
  "Find your magic journal",
  "Head to the Academy"
];
let completedObjectives = [];

// Enhanced story with character development
const storyTexts = [
  {
    chapter: 1,
    text: "Chapter 1: The Child With All Affinities and No Power\n\nYou are Kael Ardin, a 10-year-old boy in the tech-magic world of Eldoria. Born with potential for all five elements, you've trained endlessly but can't access any magic. Your parents are renowned tech-mages, and the pressure to succeed weighs heavily on you.",
    dialogues: {
      mother: "Kael, honey, how was your training today? Remember, magic comes from within. Don't give up.",
      father: "Son, the scanners show your potential is off the charts. We just need to find the right catalyst.",
      lyra: "Kael! I heard about your test. Don't listen to those doubters. I believe in you completely."
    }
  },
  {
    chapter: 2,
    text: "Chapter 2: Academy of Sparks\n\nThe Magic Academy looms ahead, a towering structure of crystal and steel. Holo-displays flicker with spell demonstrations. Students in smart uniforms hurry about. You feel the weight of expectations as you enter.",
    dialogues: {
      lyra: "There you are! Ready for our first real class? I heard they're introducing elemental basics today.",
      bully: "Hey Zero-Spark! What are you doing here? Go back to your toy wands!",
      instructor: "Welcome, students. Today we explore the harmony of magic and technology. Remember, true power comes from balance."
    }
  }
];

let currentLocation = 'home'; // home, transition, academy

applySettings();

// Show initial story on load
showStory();

function showStory() {
  const storyDiv = document.getElementById('story');
  const currentStory = storyTexts[Math.min(storyIndex, storyTexts.length - 1)];
  storyDiv.innerHTML = `
    <div style="margin-bottom:10px;">${currentStory.text}</div>
    <div style="font-size:12px;color:#aaa;">
      Current Objectives:<br>
      ${objectives.map(obj => `- ${completedObjectives.includes(obj) ? '✓' : '○'} ${obj}`).join('<br>')}
    </div>
  `;
  storyDiv.style.display = 'block';
}

function hideStory() {
  document.getElementById('story').style.display = 'none';
}

function advanceStory() {
  storyIndex = (storyIndex + 1) % storyTexts.length;
  if (storyIndex === 0) {
    showDemoEnding();
  } else {
    showStory();
  }
}

function completeObjective(obj) {
  if (!completedObjectives.includes(obj)) {
    completedObjectives.push(obj);
    showNotification(`Objective Complete: ${obj}`);
    checkChapterProgress();
  }
}

function checkChapterProgress() {
  if (currentChapter === 1 && completedObjectives.length >= 2) {
    showNotification("Chapter 1 Complete! Head to the Academy.");
    currentLocation = 'transition';
    // Add academy entrance trigger
  } else if (currentChapter === 2 && completedObjectives.length >= 4) {
    showNotification("Demo Complete! Thanks for playing.");
    advanceStory();
  }
}

function showNotification(message) {
  const notif = document.createElement('div');
  notif.style.position = 'absolute';
  notif.style.top = '50%';
  notif.style.left = '50%';
  notif.style.transform = 'translate(-50%, -50%)';
  notif.style.background = 'rgba(0,0,0,0.8)';
  notif.style.color = '#fff';
  notif.style.padding = '20px';
  notif.style.borderRadius = '10px';
  notif.style.fontSize = '18px';
  notif.textContent = message;
  document.body.appendChild(notif);
  setTimeout(() => document.body.removeChild(notif), 3000);
}

function showDialogue(character, text, choices = null) {
  const dialogueDiv = document.createElement('div');
  dialogueDiv.style.position = 'absolute';
  dialogueDiv.style.bottom = '150px';
  dialogueDiv.style.left = '50%';
  dialogueDiv.style.transform = 'translateX(-50%)';
  dialogueDiv.style.background = 'rgba(0,0,0,0.9)';
  dialogueDiv.style.color = '#fff';
  dialogueDiv.style.padding = '15px';
  dialogueDiv.style.borderRadius = '10px';
  dialogueDiv.style.maxWidth = '400px';
  dialogueDiv.innerHTML = `<strong>${character}:</strong> ${text}`;

  if (choices) {
    dialogueDiv.innerHTML += '<br><br>';
    choices.forEach(choice => {
      const btn = document.createElement('button');
      btn.textContent = choice.text;
      btn.style.margin = '5px';
      btn.style.padding = '5px 10px';
      btn.onclick = () => {
        choice.action();
        document.body.removeChild(dialogueDiv);
      };
      dialogueDiv.appendChild(btn);
    });
  } else {
    setTimeout(() => {
      if (dialogueDiv.parentNode) document.body.removeChild(dialogueDiv);
    }, 5000);
  }

  document.body.appendChild(dialogueDiv);
}

function showDemoEnding() {
  const endingDiv = document.createElement('div');
  endingDiv.style.position = 'fixed';
  endingDiv.style.top = '0';
  endingDiv.style.left = '0';
  endingDiv.style.width = '100%';
  endingDiv.style.height = '100%';
  endingDiv.style.background = 'rgba(0,0,0,0.9)';
  endingDiv.style.color = '#fff';
  endingDiv.style.display = 'flex';
  endingDiv.style.flexDirection = 'column';
  endingDiv.style.justifyContent = 'center';
  endingDiv.style.alignItems = 'center';
  endingDiv.style.fontSize = '24px';
  endingDiv.innerHTML = `
    <h1>Demo Complete!</h1>
    <p>Thank you for experiencing the beginning of Kael's journey.</p>
    <p>Your choices shaped this demo ending.</p>
    <p>Stay tuned for the full game with all 54 chapters!</p>
    <button onclick="location.reload()" style="padding:10px 20px;font-size:18px;">Play Again</button>
  `;
  document.body.appendChild(endingDiv);
}

// Particle system for magic effects
const particles = [];
function createParticle(x, y, z, color) {
  const geometry = new THREE.SphereGeometry(0.05);
  const material = new THREE.MeshBasicMaterial({ color: color });
  const particle = new THREE.Mesh(geometry, material);
  particle.position.set(x, y, z);
  particle.velocity = new THREE.Vector3((Math.random() - 0.5) * 0.1, Math.random() * 0.1, (Math.random() - 0.5) * 0.1);
  particle.life = 60; // frames
  scene.add(particle);
  particles.push(particle);
}

function updateParticles() {
  for (let i = particles.length - 1; i >= 0; i--) {
    const p = particles[i];
    p.position.add(p.velocity);
    p.life--;
    if (p.life <= 0) {
      scene.remove(p);
      particles.splice(i, 1);
    }
  }
}

function updateHUD() {
  document.getElementById('healthBar').style.width = health + '%';
  document.getElementById('manaBar').style.width = mana * 2 + '%'; // mana 0-50, so *2
  document.getElementById('staminaBar').style.width = stamina + '%';
}

// Animation loop
function animate() {
  requestAnimationFrame(animate);

  updateFPS();

  if (inCutscene) {
    // Cutscene: Kael waking up
    if (cutsceneStep < 120) { // 2 seconds at 60fps
      // Slowly sit up
      camera.position.y += 0.01;
      camera.rotation.x -= 0.005;
    } else if (cutsceneStep < 240) {
      // Stand up
      camera.position.y += 0.005;
      camera.position.z -= 0.01;
    } else {
      // End cutscene
      inCutscene = false;
      camera.position.set(0, 1.6, 5);
      camera.rotation.set(0, 0, 0);
    }
    cutsceneStep++;
  } else {
    // Normal gameplay
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

    // Reduce stamina on move
    if (controls.moveForward || controls.moveBackward || controls.moveLeft || controls.moveRight) {
      stamina = Math.max(0, stamina - 0.1);
    } else {
      stamina = Math.min(100, stamina + 0.2);
    }
  }

  // Animate objects
  crystal.rotation.x += 0.01;
  crystal.rotation.y += 0.01;
  droid.rotation.y += 0.02;

  // Animate magic crystals
  fireCrystal.rotation.x += 0.02;
  waterCrystal.rotation.y += 0.015;
  earthCrystal.rotation.z += 0.01;
  airCrystal.rotation.x += 0.025;
  spiritCrystal.rotation.y += 0.02;

  // Animate NPCs slightly
  lyra.rotation.y += 0.005;
  mother.rotation.y += 0.003;
  father.rotation.y += 0.004;

  // Pulse holograms
  scene.children.forEach(child => {
    if (child.material && child.material.opacity !== undefined) {
      child.material.opacity = 0.3 + Math.sin(Date.now() * 0.005) * 0.2;
    }
  });

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

// Raycaster for interactions
const raycaster = new THREE.Raycaster();
const mouse = new THREE.Vector2();

document.addEventListener('click', (event) => {
  mouse.x = (event.clientX / window.innerWidth) * 2 - 1;
  mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;

  raycaster.setFromCamera(mouse, camera);
  const intersects = raycaster.intersectObjects(scene.children);

  if (intersects.length > 0) {
    const object = intersects[0].object;
    if (object === crystal) {
      showDialogue("Ancient Crystal", "Touch me to awaken your dormant magic.", [
        { text: "Touch Crystal", action: () => {
          if (mana < 100) mana += 10;
          showDialogue("Kael", "I feel... something. A spark?");
          completeObjective("Find your magic journal");
        }},
        { text: "Leave it", action: () => {} }
      ]);
    } else if (object === droid) {
      showDialogue("Tech Droid", "Greetings, young mage. Your scans show potential.");
    } else if (object === lyra) {
      const dialogue = storyTexts[currentChapter - 1].dialogues.lyra;
      showDialogue("Lyra", dialogue, [
        { text: "Thank you for believing in me", action: () => {
          stamina = Math.min(100, stamina + 20);
          showDialogue("Lyra", "Of course! We're in this together.");
        }},
        { text: "I'm not sure I can do this", action: () => {
          showDialogue("Lyra", "Don't say that! I know you can. Let me help.");
        }}
      ]);
    } else if (object === mother) {
      const dialogue = storyTexts[currentChapter - 1].dialogues.mother;
      showDialogue("Mother", dialogue);
      completeObjective("Explore your home and interact with your parents");
    } else if (object === father) {
      const dialogue = storyTexts[currentChapter - 1].dialogues.father;
      showDialogue("Father", dialogue);
    } else if (object.element) {
      showDialogue(`${object.element.charAt(0).toUpperCase() + object.element.slice(1)} Crystal`, "Channel my power, Kael.");
      mana += 5;
    } else if (object === academy) {
      showDialogue("Academy Entrance", "The doors slide open. Welcome to the Academy of Sparks.");
      currentLocation = 'academy';
      currentChapter = 2;
      objectives = [
        "Meet Lyra at the Academy",
        "Attend your first class",
        "Stand up to bullies"
      ];
      completedObjectives = [];
      showStory();
    } else if (object === bed) {
      showDialogue("Kael", "My bed... where it all begins each day.");
    }
  } else {
    // Cast spell if mana available
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
  }
});

function showDialogue(text) {
  const dialogueDiv = document.createElement('div');
  dialogueDiv.style.position = 'absolute';
  dialogueDiv.style.bottom = '100px';
  dialogueDiv.style.left = '50%';
  dialogueDiv.style.transform = 'translateX(-50%)';
  dialogueDiv.style.background = 'rgba(0,0,0,0.8)';
  dialogueDiv.style.color = '#fff';
  dialogueDiv.style.padding = '10px';
  dialogueDiv.style.borderRadius = '5px';
  dialogueDiv.style.fontSize = '16px';
  dialogueDiv.textContent = text;
  document.body.appendChild(dialogueDiv);
  setTimeout(() => document.body.removeChild(dialogueDiv), 3000);
}

// Settings UI
document.getElementById('settingsBtn').addEventListener('click', () => {
  const panel = document.getElementById('settingsPanel');
  panel.style.display = panel.style.display === 'none' ? 'block' : 'none';
  // Update UI to match current settings
  document.getElementById('invertY').checked = settings.invertY;
  document.getElementById('sensitivity').value = settings.sensitivity;
  document.getElementById('showFPS').checked = settings.showFPS;
  document.getElementById('volume').value = settings.volume;
  document.getElementById('shadows').checked = settings.shadows;
});

document.getElementById('closeSettings').addEventListener('click', () => {
  document.getElementById('settingsPanel').style.display = 'none';
});

// Update settings on change
document.getElementById('invertY').addEventListener('change', (e) => {
  settings.invertY = e.target.checked;
  saveSettings();
});

document.getElementById('sensitivity').addEventListener('input', (e) => {
  settings.sensitivity = parseFloat(e.target.value);
  saveSettings();
});

document.getElementById('showFPS').addEventListener('change', (e) => {
  settings.showFPS = e.target.checked;
  applySettings();
  saveSettings();
});

document.getElementById('volume').addEventListener('input', (e) => {
  settings.volume = parseFloat(e.target.value);
  saveSettings();
});

document.getElementById('shadows').addEventListener('change', (e) => {
  settings.shadows = e.target.checked;
  applySettings();
  saveSettings();
});

function saveSettings() {
  localStorage.setItem('gameSettings', JSON.stringify(settings));
}

// FPS counter
let fps = 0;
let lastTime = performance.now();
function updateFPS() {
  const now = performance.now();
  fps = Math.round(1000 / (now - lastTime));
  lastTime = now;
  if (settings.showFPS) {
    document.getElementById('fpsDisplay').textContent = `FPS: ${fps}`;
  }
}