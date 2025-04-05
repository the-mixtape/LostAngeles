<template>
  <QuickSlots v-if="showQuickSlots" :slots="quickSlotsData"/>
</template>

<script setup>
import {ref, onMounted, onUnmounted} from "vue";
import QuickSlots from "@/components/QuickSlots.vue";

const showQuickSlots = ref(false);
const quickSlotsData = ref(Array(10).fill({ item: null })); // Data Example

function onShowInventory(itemData) {
  let display = itemData.Display;
  console.log("Show Inventory: ", display);
}

function onShowQuickSlots(itemData) {
  showQuickSlots.value = itemData.Display;
  console.log("Show Quick Slots: ", showQuickSlots.value);
}

const handlers = {
  showInventory: onShowInventory,
  showQuickSlots: onShowQuickSlots
};

const handleMessageListener = (event) => {
  const itemData = event?.data;
  const handlerType = itemData.Type;
  if (handlers[handlerType]) handlers[handlerType](itemData);
};

const closeUI = () => {
  fetch(`https://${GetParentResourceName()}/Inventory::Close`, {
    method: "POST",
    headers: {"Content-Type": "application/json"},
    body: JSON.stringify({})
  }).then(() => {
    showQuickSlots.value = false;
  });
};

const handleKeyDown = (event) => {
  if (event.key === "Escape" || event.key === "Tab" || event.key === "i" || event.key === "I") {
    closeUI();
    event.preventDefault();
  }
};

onMounted(() => {
  window.addEventListener("keydown", handleKeyDown);
  window.addEventListener("message", handleMessageListener);
});

onUnmounted(() => {
  window.removeEventListener("keydown", handleKeyDown);
  window.removeEventListener("message", handleMessageListener);
});
</script>

<style scoped>
</style>
