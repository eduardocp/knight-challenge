<script setup lang="ts">
import axios from 'axios'
import { ref, onMounted } from 'vue'
import { Button } from '@/components/ui/button';
import { Pencil1Icon, TrashIcon, ReloadIcon } from '@radix-icons/vue'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { AutoForm, AutoFormField } from '@/components/ui/auto-form'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog'
import Knight from '@/models/knight';
import { useToast } from '@/components/ui/toast/use-toast'
import * as z from 'zod'
import { toTypedSchema } from '@vee-validate/zod'
import { useForm } from 'vee-validate'
import { parseAbsolute } from '@internationalized/date';

enum AttributeTypes {
  strength = "Strength",
  dexterity = "Dexterity",
  constitution = "Constitution",
  intelligence = "Intelligence",
  wisdom = "Wisdom",
  charisma = "Charisma",
}

const formSchema = z.object({
  name: z.string().min(2).max(50),
  nickName: z.string().min(2).max(50),
  birthday: z.any().transform((str, ctx): z.infer<ReturnType<any>> => {
    try {
      return str.toString();
    } catch (e) {
      ctx.addIssue({ code: 'custom', message: 'Invalid Date' })
      return z.NEVER
    }
  }),
  weapons: z
    .array(
      // Define the fields for each item
      z.object({
        name: z.string(),
        mod: z.number(),
        attr: z.nativeEnum(AttributeTypes),
        equipped: z.boolean(),
      })
    ).min(1),
  attributes: z.object({
    strength: z.number().min(0),
    dexterity: z.number().min(0),
    constitution: z.number().min(0),
    intelligence: z.number().min(0),
    wisdom: z.number().min(0),
    charisma: z.number().min(0),
  }),
  keyAttribute: z.nativeEnum(AttributeTypes)
});

const form = useForm({
  validationSchema: toTypedSchema(formSchema),
});

function stringToAttributeType(attr: string): AttributeTypes {
  switch (attr.toLowerCase()) {
    case 'strength':
      return AttributeTypes.strength;
    case 'dexterity':
      return AttributeTypes.dexterity;
    case 'constitution':
      return AttributeTypes.constitution;
    case 'intelligence':
      return AttributeTypes.intelligence;
    case 'wisdom':
      return AttributeTypes.wisdom;
    case 'charisma':
      return AttributeTypes.charisma;
    default:
      throw new Error(`Unknown attribute type: ${attr}`);
  }
}

function setForm(knight: Knight) {
  console.log(knight.birthday)
  console.dir(parseAbsolute(knight.birthday, 'UTC'))
  form.resetForm()
  form.setFieldValue('name', knight.name)
  form.setFieldValue('nickName', knight.nickName)
  form.setFieldValue('birthday', parseAbsolute(knight.birthday, 'UTC'))
  form.setFieldValue('attributes.charisma', knight.attributes.charisma)
  form.setFieldValue('attributes.constitution', knight.attributes.constitution)
  form.setFieldValue('attributes.dexterity', knight.attributes.dexterity)
  form.setFieldValue('attributes.intelligence', knight.attributes.intelligence)
  form.setFieldValue('attributes.strength', knight.attributes.strength)
  form.setFieldValue('attributes.wisdom', knight.attributes.wisdom)



  knight.weapons.map((weapon, index) => {
    form.setFieldValue(`weapons.${index}.name`, weapon.name)
    form.setFieldValue(`weapons.${index}.mod`, weapon.mod)
    form.setFieldValue(`weapons.${index}.attr`, stringToAttributeType(weapon.attr))
    form.setFieldValue(`weapons.${index}.equipped`, weapon.equipped)
  });

  form.setFieldValue('keyAttribute', stringToAttributeType(knight.keyAttribute))

}

function onSubmitCreation(values: Record<string, any>, id: string | undefined) {
  creating.value = true;
  axios.post('https://localhost:8081/knights', values)
    .then(response => {
      toast({
        title: 'Knight successful registered.'
      })
      knights.value?.push(response.data as Knight)
      isOpenCreateModel.value = false
    })
    .catch(function (error) {
      toast({
        title: 'Ooops.',
        description: error.response.data.message,
        variant: 'destructive',
      })
    }).finally(() => creating.value = false)
}

function deleteKnight(id: string) {
  deleting.value.push(id);
  axios.delete(`https://localhost:8081/knights/${id}`).then(response => response.data).then(_ => toast({
    title: 'Knight successful unregistered.'
  })).finally(() => {
    let index = deleting.value.indexOf(id);
    deleting.value = deleting.value.splice(index, 1);
    knights.value = knights.value?.filter(x => x.id != id);
  });
}

const knights = ref<Knight[]>();
const creating = ref<boolean>(false);
const deleting = ref<string[]>([]);
const editing = ref<string[]>([]);
const isOpenCreateModel = ref<boolean>(false);
const { toast } = useToast();

function getData() {
  knights.value = undefined;
  axios.get('https://localhost:8081/knights').then(response => response.data).then(data => knights.value = data);
}

onMounted(() => {
  getData();
})

</script>

<template>
  <div class="hidden h-full flex-1 flex-col space-y-8 p-8 md:flex">
    <div class="flex items-center justify-between space-y-2">
      <div>
        <h2 class="text-2xl font-bold tracking-tight">
          Knights
        </h2>
        <p class="text-muted-foreground">
          Here&apos;s a list of knights registered on your tavern!
        </p>
      </div>
      <div class="flex items-center space-x-2">

        <Dialog v-model:open="isOpenCreateModel">
          <DialogTrigger as-child>
            <Button :disabled="creating">
              <ReloadIcon v-if="creating" class="w-4 h-4 mr-2 animate-spin" /> New Knight
            </Button>
          </DialogTrigger>
          <DialogContent class="sm:max-w-[30%]">
            <DialogHeader>
              <DialogTitle>New Knight</DialogTitle>
            </DialogHeader>
            <AutoForm class="w-full space-y-4 flex flex-wrap gap-2" :schema="formSchema"
              @submit="(values) => onSubmitCreation(values, undefined)" :field-config="{
                birthday: {
                  component: 'date',
                },
              }">
              <template #name="slotProps">
                <div class="w-[49%] space-x-2">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <template #nickName="slotProps">
                <div class="w-[49%] space-x-2" style="margin-top: 0px !important;">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <template #birthday="slotProps">
                <div class="w-full space-x-2">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <template #weapons="slotProps">
                <div class="w-[49%] space-x-2">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <template #attributes="slotProps">
                <div class="w-[49%] space-x-2">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <template #keyAttribute="slotProps">
                <div class="w-full space-x-2">
                  <AutoFormField v-bind="slotProps" />
                </div>
              </template>

              <Button type="submit" :disabled="creating">
                <ReloadIcon v-if="creating" class="w-4 h-4 mr-2 animate-spin" /> Submit
              </Button>
            </AutoForm>
          </DialogContent>
        </Dialog>
      </div>
    </div>

    <Table v-if="knights">
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>Age</TableHead>
          <TableHead>Weapons</TableHead>
          <TableHead>Attribute</TableHead>
          <TableHead>Attack</TableHead>
          <TableHead>Experience</TableHead>
          <TableHead></TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        <TableRow v-for="knight in knights" :key="knight.id">
          <TableCell class="font-medium">{{ knight.name }}</TableCell>
          <TableCell>{{ knight.age }}</TableCell>
          <TableCell>{{ knight.weapons.length }}</TableCell>
          <TableCell>{{ knight.keyAttribute }}</TableCell>
          <TableCell>{{ knight.attack }}</TableCell>
          <TableCell>{{ knight.experience }}</TableCell>
          <TableCell class="text-right">
            <Dialog>
              <DialogTrigger as-child>
                <Button :disabled="editing.indexOf(knight.id) > -1" size="icon" v-on:click="setForm(knight)"
                  class="mr-2">
                  <ReloadIcon v-if="editing.indexOf(knight.id) > -1" class="w-4 h-4" />
                  <Pencil1Icon v-else class="w-4 h-4" />
                </Button>
              </DialogTrigger>
              <DialogContent class="sm:max-w-[30%]">
                <DialogHeader>
                  <DialogTitle>Edit Knight {{ knight.name }} ({{ knight.id }})</DialogTitle>
                </DialogHeader>
                <AutoForm class="w-full space-y-4 flex flex-wrap gap-2" :form="form" :schema="formSchema"
                  @submit="(values) => onSubmitCreation(values, knight.id)" :field-config="{
                    birthday: {
                      component: 'date'
                    },
                  }">
                  <template #name="slotProps">
                    <div class="w-[49%] space-x-2">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <template #nickName="slotProps">
                    <div class="w-[49%] space-x-2" style="margin-top: 0px !important;">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <template #birthday="slotProps">
                    <div class="w-full space-x-2">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <template #weapons="slotProps">
                    <div class="w-[49%] space-x-2">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <template #attributes="slotProps">
                    <div class="w-[49%] space-x-2">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <template #keyAttribute="slotProps">
                    <div class="w-full space-x-2">
                      <AutoFormField v-bind="slotProps" />
                    </div>
                  </template>

                  <Button type="submit" :disabled="creating">
                    <ReloadIcon v-if="creating" class="w-4 h-4 mr-2 animate-spin" /> Submit
                  </Button>
                </AutoForm>
              </DialogContent>
            </Dialog>
            <AlertDialog>
              <AlertDialogTrigger as-child>
                <Button variant="destructive" size="icon" title="Edit" :disabled="deleting.indexOf(knight.id) > -1">
                  <ReloadIcon v-if="deleting.indexOf(knight.id) > -1" class="w-4 h-4" />
                  <TrashIcon v-else class="w-4 h-4" />
                </Button>
              </AlertDialogTrigger>
              <AlertDialogContent>
                <AlertDialogHeader>
                  <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
                  <AlertDialogDescription>
                    This action cannot be undone. This will permanently remove
                    the knight '<span style="font-weight: bold;">{{ knight.name }}</span>' from our books!
                  </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                  <AlertDialogCancel>Cancel</AlertDialogCancel>
                  <AlertDialogAction v-on:click="deleteKnight(knight.id)">Yes :(
                  </AlertDialogAction>
                </AlertDialogFooter>
              </AlertDialogContent>
            </AlertDialog>
          </TableCell>
        </TableRow>
      </TableBody>
    </Table>
    <div v-else>
      <div class="flex items-center">
        <ReloadIcon class="w-4 h-4 mr-2 animate-spin" /> Loading...
      </div>
    </div>
  </div>
</template>

<style scoped></style>
