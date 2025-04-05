<template>
  <div class="quick-slots">
    <div v-for="(slot, index) in slots" :key="index" class="quick-slot">
      {{ slot.Item ? slot.Item.Name : '' }}
    </div>
  </div>
</template>

<script setup>
const props = defineProps({
  slots: {
    type: Array,
    default: () => Array(10).fill({Item: null})
  }
})
</script>

<style scoped>
.quick-slots {
  position: fixed;
  bottom: 20px;
  left: 50%;
  transform: translateX(-50%);
  display: flex;
  gap: 10px;
  background: rgba(0, 0, 0, 0.7);
  padding: 8px;
  border-radius: 5px;
}

.quick-slot {
  width: 50px;
  height: 50px;
  background: rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 3px;
}
</style>