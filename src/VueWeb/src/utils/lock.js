export default function lock() {
  this.locked = null
}

lock.prototype.lock = async function () {
  if (this.locked === null) this.locked = []
  else await new Promise((res) => this.locked.push(res))
}

lock.prototype.unlock = function () {
  if (this.locked.length) this.locked.shift()
  else this.locked = null
}

lock.prototype.isLocked = async function () {
    return await new Promise(res => {
        return this.locked !== null
    })
}